using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;
using AndroidX.AppCompat.App;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.Gms.Common.Apis;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YeutzLi
{
    [Activity(Label = "CounselorActivity")]
    public class CounselorActivity : Activity
    {
        EditText userInput;
        Button sendButton;
        TextView chatResponse;
        HttpClient client = new HttpClient();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layoutCounselor);
            // Create your application here

            userInput = FindViewById<EditText>(Resource.Id.userInput);
            sendButton = FindViewById<Button>(Resource.Id.sendButton);
            chatResponse = FindViewById<TextView>(Resource.Id.chatResponse);
            

            sendButton.Click += async (sender, e) =>
            {
                string message = userInput.Text.Trim();
                if (!string.IsNullOrEmpty(message))
                {
                    userInput.Text = "";  // Clear input
                    chatResponse.Text = "Thinking...";  // Show loading text

                    var result = await GetAIResponse(message);
                    string response = JsonConvert.SerializeObject(result, Formatting.Indented);
                    chatResponse.Text = response;  // Display response
                }
            };
        }

        public static async Task<object> GetAIResponse(string prompt)
        {
            string url = "https://api.restful-api.dev/objects";

            var data = new
            {
                model = "gemma3",
                prompt = prompt
            };


            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);

                // Read the response content
                var responseContent = await response.Content.ReadAsStringAsync();

                // Parse the response as a JObject
                var jsonResponse = JObject.Parse(responseContent);

                // Return the extracted value (it could be any type based on the API response)
                return jsonResponse;
            }
        }


    }
}
