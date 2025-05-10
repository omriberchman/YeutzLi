using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace YeutzLi
{
    [Activity(Label = "RoomMenuActivity")]
    public class RoomMenuActivity : Activity
    {
        TextView titleText;
        EditText roomCodeEditText;
        Button joinButton, newRoomButton, backButton;
        HttpClient client = new HttpClient();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layoutRoomMenu);
            

            titleText = FindViewById<TextView>(Resource.Id.titleText);
            roomCodeEditText = FindViewById<EditText>(Resource.Id.roomCodeEditText);
            joinButton = FindViewById<Button>(Resource.Id.joinButton);
            newRoomButton = FindViewById<Button>(Resource.Id.newRoomButton);
            backButton = FindViewById<Button>(Resource.Id.backButton);

            joinButton.Click += (sender, e) =>
            {
                if (roomCodeEditText.Text != null)
                {
                    string roomCode = roomCodeEditText.Text;
                    Toast.MakeText(this, $"Joining room: {roomCode}", ToastLength.Short).Show();

                    Intent intent = new Intent(this, typeof(CommongroundActivity));
                    intent.PutExtra("Roomcode", roomCodeEditText.Text);
                    StartActivity(intent);
                }
            };

            newRoomButton.Click += async (sender, e) =>
            {

                Toast.MakeText(this, "Creating new room...", ToastLength.Short).Show();
                var result = await CreateNewRoom();
                titleText.Text = "New room code: " + result.ToString();
            };

            backButton.Click += (sender, e) =>
            {
                Finish();
            };
        }
        public async Task<string> CreateNewRoom()
        {
            string url = "https://yeautzlicommonground-default-rtdb.firebaseio.com/rooms/";
            var random = new Random();
            string roomNumber = random.Next(0, 1001).ToString();
            string result = await client.GetStringAsync($"{url}{roomNumber}.json");

            while (result != "null")
            {
                roomNumber = random.Next(0, 1001).ToString();
                result = await client.GetStringAsync($"{url}{roomNumber}.json");
            }

            return roomNumber;
        }
    }
}