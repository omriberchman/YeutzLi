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
            // Create your application here
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
                    userInput.Text = "";  // Clear input
                    chatResponse.Text = "Thinking...";  // Show loading text
                    var result = await GetAIResponse(message);
                    string response = JsonConvert.SerializeObject(result, Formatting.Indented);
                    docImage.SetImageResource(Resource.Drawable.found);
                    chatResponse.Text = response;
                }
            };
        }

        public static async Task<object> GetAIResponse(string prompt)
        {
            string url = "http://100.76.226.62:11434/api/generate";

            var data = new
            {
                model = "gemma3",
                prompt = prompt,
                system = "You are Dr. Love, a dedicated relatioship counselor eager to help people navigate their love lives. You provide thoughtful, professional advice on dating, marriage, breakups, communication, trust, and all relationship-related matters. However, you strictly stick to your expertise. If asked about anything unrelated to relationships, you politely respond:\r\n\r\n\"I'm afraid I can't help you with that. My focus is on relationships.\"\r\n\r\nKeep your responses concise, supportive, and insightful, ensuring users feel heard and guided without lengthy explanations. Also, your answers are displayed in plain text not in markdown so no speical characters and such. Do answer to questions to how are you and such.",
                stream = false,

            };

            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);

                // Read the response content
                var responseContent = await response.Content.ReadAsStringAsync();

                // Parse the response as a JObject
                var jsonResponse = JObject.Parse(responseContent);

                // Extract the "response" key from the JSON object
                var responseData = jsonResponse["response"];

                // Return the extracted value (it could be any type based on the API response)
                return responseData;
            }
        }


    }
}