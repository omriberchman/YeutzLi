using Android.Gms.Extensions;
using Firebase.Auth;
using Firebase.Firestore;
using Java.Util;
using System.Threading.Tasks;

namespace YeutzLi
{
    public class User
    {
        string Email{ set; get; }
        string Password { set; get; }
        int Age { set; get; }

        public User(string email, string password, int age)
        {
            Email = email;
            Password = password;
            Age = age;
        }
        public User(string email, string password)
        {
            Email = email;
            Password = password;
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