using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YeutzLi
{
    public class FireBaseHelper
    {
        public static FirebaseFirestore db;   // קישור לפייר בייס
        public static FirebaseAuth auth; // קישור לפיירבייס התחברות
      
        public static FirebaseFirestore GetFirestore() //מחזירה קישור ל- FirebaseFirestore
        {
            if (db != null)
            {
                return db;
            }
            var app = FirebaseApp.InitializeApp(Application.Context);

            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("omriyeutzli")
                    .SetApplicationId("omriyeutzli")
                    .SetApiKey("AIzaSyAKs4Yh2NBvB6ZwPI3Z0zaae8yfTqYzXzM")
                    .SetDatabaseUrl("https://omriyeutzli.firebaseio.com")
                    .SetStorageBucket("omriyeutzli.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(Application.Context, options, "YeutzLi");
                //FirebaseApp.InitializeApp(context, options, "MarketList");
                db = FirebaseFirestore.GetInstance(app);
            }
            else
            {
                db = FirebaseFirestore.GetInstance(app);
            }
            return db;
        }
       
        public static FirebaseAuth GetFirebaseAuthentication() //מחזירה קישור ל- FirebaseAuth
        {

            var app = FirebaseApp.InitializeApp(Application.Context);
            if (app == null)
            {
                var options = new FirebaseOptions.Builder()
                    .SetProjectId("omriyeutzli")
                    .SetApplicationId("omriyeutzli")
                    .SetApiKey("AIzaSyAKs4Yh2NBvB6ZwPI3Z0zaae8yfTqYzXzM")
                    .SetDatabaseUrl("https://omriyeutzli.firebaseio.com")
                    .SetStorageBucket("omriyeutzli.appspot.com")
                    .Build();

                app = FirebaseApp.InitializeApp(Application.Context, options);
                auth = FirebaseAuth.Instance;
            }
            else
            {
                auth = FirebaseAuth.Instance;
            }
            return auth;
        }
    }
}