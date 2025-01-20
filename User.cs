using Android.App;
using Android.Content;
using Android.Gms.Extensions;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Auth;
using Firebase.Firestore;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YeutzLi
{
    public class User
    {
        string Email{ set; get; }
        string Password { set; get; }
        string Age { set; get; }

        public User(string email, string password, string age)
        {
            Email = email;
            Password = password;
            Age = age;
        }
        public async Task<bool> Register()
        {
            FirebaseAuth firebaseAuthentication = FireBaseHelper.GetFirebaseAuthentication();
            FirebaseFirestore database = FireBaseHelper.GetFirestore();
            try
            {
                await firebaseAuthentication.CreateUserWithEmailAndPassword(this.Email, this.Password);
                
            }
            catch
            {
                return false;
            }


            try
            {
                HashMap userMap = new HashMap();
                userMap.Put("Email", this.Email);
                userMap.Put("Password", this.Password);
                //  userMap.Put("Id", firebaseAuthentication.CurrentUser.Uid);
                DocumentReference userReference = database.Collection("User").Document();
                await userReference.Set(userMap);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public async Task<bool> Login()
        {
            FirebaseAuth firebaseAuthentication = FireBaseHelper.GetFirebaseAuthentication();

            try
            {
                await firebaseAuthentication.SignInWithEmailAndPassword(this.Email, this.Password);

            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}