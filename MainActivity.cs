using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Android.Content;
using System;

namespace YeutzLi
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btMainReg, loginButton;
        EditText emailField, passwordField;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            // Find the button by ID
            Button loginButton = FindViewById<Button>(Resource.Id.loginButton);
            emailField = FindViewById<EditText>(Resource.Id.emailField);
            passwordField = FindViewById<EditText>(Resource.Id.passwordField);

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

        private async void LoginButton_Click(object sender, System.EventArgs e)
        {
            String email = emailField.Text;
            String password = passwordField.Text;

            User user = new User(email, password);
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                Toast.MakeText(this, "Please fill all fields", ToastLength.Short).Show();
                return;
            }

            if (await user.Login())
            {
                Toast.MakeText(this, "Successfully registered", ToastLength.Short).Show();
                Intent intent = new Intent(this, typeof(HomeActivity));
                StartActivity(intent);
            }
            else
            {
                Toast.MakeText(this, "Error accrued while registering. Try again..", ToastLength.Short).Show();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
    }




   
}