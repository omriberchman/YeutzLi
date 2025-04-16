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
using System.Collections.Generic;

namespace YeutzLi
{
    [Activity(Label = "CounselorActivity")]
    public class CounselorActivity : Activity
    {
        EditText userInput;
        Button sendButton;
        TextView chatResponse;
        ImageView docImage;
        HttpClient client = new HttpClient();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layoutCounselor);
            userInput = FindViewById<EditText>(Resource.Id.userInput);
            sendButton = FindViewById<Button>(Resource.Id.sendButton);
            chatResponse = FindViewById<TextView>(Resource.Id.chatResponse);
            docImage = FindViewById<ImageView>(Resource.Id.docImage);

            sendButton.Click += async (sender, e) =>
            {
                string message = userInput.Text.Trim();
                if (!string.IsNullOrEmpty(message))
                {
                    docImage.SetImageResource(Resource.Drawable.thinking);
                    userInput.Text = "";  
                    chatResponse.Text = "Thinking...";  
                    var result = await GetAIResponse(message);
                    string response = JsonConvert.SerializeObject(result, Formatting.Indented);
                    docImage.SetImageResource(Resource.Drawable.found);
                    chatResponse.Text = response;
                }
            };
        }

        public static async Task<object> GetAIResponse(string prompt)
        {
            string chatHistoryFilePath = Path.Combine(Application.Context.FilesDir.AbsolutePath, "chatHistory.json");

            // Read existing chat history or create an empty list if the file doesn't exist
            List<Dictionary<string, string>> chatHistory = new List<Dictionary<string, string>>();

            if (File.Exists(chatHistoryFilePath))
            {
                var existingHistory = File.ReadAllText(chatHistoryFilePath);
                chatHistory = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(existingHistory);
            }

            
            // Prepare data to send to the API
            string url = "http://100.76.226.62:11434/api/chat";
            var data = new
            {
                model = "gemma3",
                prompt,
                messages = chatHistory,
                stream = false
            };

            // Add the user's prompt to chat history
            chatHistory.Add(new Dictionary<string, string>
            {
                { "role", "user" },
                { "content",  prompt}
            });

            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);

                // Read the response content
                var responseContent = await response.Content.ReadAsStringAsync();

                // Parse the response as a JObject
                var jsonResponse = JObject.Parse(responseContent);

                // Extract the "response" key from the JSON object
                var responseData = jsonResponse["message"]["content"].ToString();

                // Add the AI's response to chat history
                chatHistory.Add(new Dictionary<string, string>
                {
                    { "role", "assistant" },
                    { "content", responseData }
                });

                // Save the updated chat history back to the file
                File.WriteAllText(chatHistoryFilePath, JsonConvert.SerializeObject(chatHistory, Formatting.Indented));

                // Return the AI's response
                return responseData;
            }
        }

    }
}