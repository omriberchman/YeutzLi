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
    [Activity(Label = "HomeActivity")]
    public class HomeActivity : Activity
    {
        Button btHomeLogout;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layoutHome);
            // Create your application here
            btHomeLogout = FindViewById<Button>(Resource.Id.btHomeLogout);

            // Set the click event
            btHomeLogout.Click += BtHomeLogout_Click;
          
        }

        private void BtHomeLogout_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(CounselorActivity));
            StartActivity(intent);
            Finish();
        }
    }
}