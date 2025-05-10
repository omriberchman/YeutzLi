using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeutzLi
{
    [Activity(Label = "ActivityRegister")]
    public class ActivityRegister : Activity
    {
        Button reg_submit, reg_exit;
        EditText reg_email , reg_password   , reg_age;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layoutRegister);
            // Create your application here  layoutRegister
            reg_email = FindViewById<EditText>(Resource.Id.reg_email);
            reg_password = FindViewById<EditText>(Resource.Id.reg_password);
            reg_age = FindViewById<EditText>(Resource.Id.reg_age);
            reg_submit = FindViewById<Button>(Resource.Id.reg_submit);
            reg_exit = FindViewById<Button>(Resource.Id.reg_exit);
            reg_exit.Click += (sender, e) =>
            {
                Finish();
            };
            reg_submit.Click += OnSubmitClicked;
        }

        private async void OnSubmitClicked(object sender, EventArgs e)
        {
            string email = reg_email.Text;
            string password = reg_password.Text;
            int ageInt = Int32.Parse(reg_age.Text);

            // בדיקת אם כל השדות הוזנו
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || ageInt == null)
            {
                Toast.MakeText(this, "Please fill all fields", ToastLength.Short).Show();
                return;
            }
            if (ageInt < 18)
            {
                Toast.MakeText(this, "Must be 18 years old or older.", ToastLength.Short).Show();
                return;
            }
            User user=new User(email,password);  
            if(await user.Register())
            {
                Toast.MakeText(this, "Successfully registered", ToastLength.Short).Show();
                Finish();
            }
            else
            {
                Toast.MakeText(this, "Error accrued while registering. Try again..", ToastLength.Short).Show();
            }
        }
    }
}