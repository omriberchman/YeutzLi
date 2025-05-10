using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;
using AndroidX.AppCompat.App;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.ComponentModel;


namespace YeutzLi
{
    [Activity(Label = "CommongroundActivity")]
    public class CommongroundActivity : Activity
    {
        EditText userInput;
        Button sendButton, refreshButton;
        TextView chatResponse;
        ImageView docImage;
        HttpClient client = new HttpClient();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layoutCommonground);

            userInput = FindViewById<EditText>(Resource.Id.userInput);
            sendButton = FindViewById<Button>(Resource.Id.sendButton);
            refreshButton = FindViewById<Button>(Resource.Id.refreshButton);
            chatResponse = FindViewById<TextView>(Resource.Id.chatResponse);
            docImage = FindViewById<ImageView>(Resource.Id.docImage);

            chatResponse.Text = "Welcome to room " + Intent.GetStringExtra("Roomcode") + "!\nI'm here to help you find a commonground\nbetween you and your partner.\n Type your part in the textbox below!";

            sendButton.Click += async (sender, e) =>
            {
                string message = userInput.Text.Trim();
                if (!string.IsNullOrEmpty(message))
                {
                    docImage.SetImageResource(Resource.Drawable.thinking);
                    chatResponse.Text = "...";
                    int response = await UploadPart(userInput.Text, Intent.GetStringExtra("Roomcode"));
                    userInput.Text = "";
                    if (response == 1)
                    {
                        chatResponse.Text = "Send second part and press refresh";
                    }
                    else if (response == 2)
                    {
                        chatResponse.Text = "Completed! Press the refresh button to see the results!";
                    }
                    else if (response == 3)
                    {
                        chatResponse.Text = "Two parts already provided!";
                    }
                }
            };
            refreshButton.Click += async (sender, e) =>
            {
                HttpResponseMessage getResponse = await client.GetAsync($"https://yeautzlicommonground-default-rtdb.firebaseio.com/rooms/{Intent.GetStringExtra("Roomcode")}.json");
                string jsonData = await getResponse.Content.ReadAsStringAsync();
                JObject data = (jsonData == "null") ? new JObject() : JObject.Parse(jsonData);
                if (data["response"] != null)
                {
                    docImage.SetImageResource(Resource.Drawable.found);
                    chatResponse.Text = data["response"].ToString();
                }
                else
                {
                    chatResponse.Text = "No response exits!";
                }
            };
            async Task<int> UploadPart(string part, string roomCode)
            {
                string url = $"https://yeautzlicommonground-default-rtdb.firebaseio.com/rooms/{roomCode}.json";

                using (HttpClient client = new HttpClient())
                {
                    // Get current parts
                    HttpResponseMessage getResponse = await client.GetAsync(url);
                    string jsonData = await getResponse.Content.ReadAsStringAsync();
                    JObject data = (jsonData == "null") ? new JObject() : JObject.Parse(jsonData);

                    // Determine if part1 or part2 exists
                    JObject upload;
                    if (!data.ContainsKey("part1"))
                    {
                        upload = new JObject { ["part1"] = part };
                    }
                    else if (!data.ContainsKey("part2"))
                    {
                        upload = new JObject { ["part2"] = part };
                    }
                    else
                    {
                        return 3;
                    }

                    StringContent content = new StringContent(JsonConvert.SerializeObject(upload), Encoding.UTF8, "application/json");
                    await client.PatchAsync(url, content);

                    HttpResponseMessage updatedResponse = await client.GetAsync(url);
                    string updatedJson = await updatedResponse.Content.ReadAsStringAsync();
                    JObject updated = (updatedJson == "null") ? new JObject() : JObject.Parse(updatedJson);

                    if (updated.ContainsKey("part1") && updated.ContainsKey("part2"))
                    {
                        AIupdate(updated["part1"].ToString(), updated["part2"].ToString(), Intent.GetStringExtra("Roomcode")); 
                        return 2;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }

            async void AIupdate(string part1, string part2, string Roomcode)
            {
                var url = "http://100.76.226.62:11434/api/generate";
                var prompt = $"Find the common ground between these two parts: {part1} and {part2}";

                var data = new
                {
                    model = "gemma3",
                    prompt = prompt,
                    system = "Your task is to find common ground between two or more different statements or preferences. When given differing inputs, suggest a creative, practical, or thoughtful idea or activity that unites them in a meaningful way. Always aim to connect the perspectives without favoring one over the other. For example, if one says 'I like flowers' and another says 'I like the indoors', you could suggest 'You could visit a flower museum' Keep responses concise, positive, and focused on overlapping possibilities. Your response is displayed in raw text so no markdown. You may not add any other info than the answer itself.",
                    stream = false
                };

                using (var client = new HttpClient())
                {
                    var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    var responseBody = await response.Content.ReadAsStringAsync();

                    var json = JObject.Parse(responseBody);
                    var aiResponse = (string)json["response"];

                    var patchData = new { response = aiResponse };
                    var patchContent = new StringContent(JsonConvert.SerializeObject(patchData), Encoding.UTF8, "application/json");

                    var patchUrl = $"https://yeautzlicommonground-default-rtdb.firebaseio.com/rooms/{Roomcode}.json";
                    var patchRequest = new HttpRequestMessage(new HttpMethod("PATCH"), patchUrl)
                    {
                        Content = patchContent
                    };

                    await client.SendAsync(patchRequest);

                }
            }
        }
    }
}