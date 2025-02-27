using Android.App;
using Android.OS;
using Android.Widget;
using Android.Views;
using AndroidX.AppCompat.App;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;

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

                    string response = await GetAIResponse(message);
                    chatResponse.Text = response;  // Display response
                }
            };
        }

        public static async Task<string> GetAIResponse(string requestBody)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Set up the request content with the provided body
                    var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

                    // Send the POST request to the API
                    HttpResponseMessage response = await client.PostAsync("https://192.168.0.26:11434/api/generate", content);

                    // Ensure the response is successful
                    response.EnsureSuccessStatusCode();

                    // Read the response as a string
                    string responseString = await response.Content.ReadAsStringAsync();

                    // Return the response string
                    return responseString;
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur during the request
                    return $"Error: {ex.Message}";
                }
            }
        }


    }
}
