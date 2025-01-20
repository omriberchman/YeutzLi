using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Android.Content;
using System;
using Xamarin.Essentials;
using Java.Net;

namespace YeutzLi
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btMainReg;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Find the button by ID
            Button loginButton = FindViewById<Button>(Resource.Id.loginButton);
          
            // Set the click event
            loginButton.Click += LoginButton_Click;

            btMainReg = FindViewById<Button>(Resource.Id.btMainReg);
            btMainReg.Click += BtMainReg_Click;

        }

        private void BtMainReg_Click(object sender, System.EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ActivityRegister));
            StartActivity(intent);
        }

        private void LoginButton_Click(object sender, System.EventArgs e)
        {
            if (true)
            {
                Intent intent = new Intent(this, typeof(HomeActivity));
                StartActivity(intent);
            }
            else
            {
                Console.WriteLine("Username or password incorrect.");
            }
           // Finish();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
    }




   
}