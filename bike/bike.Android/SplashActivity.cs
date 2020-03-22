
using Android.App;
using Android.Support.V7.App;

namespace bike.Droid
{
    [Activity(Label = "Mobile App Name", Icon = "@drawable/icon", Theme = "@style/SplashTheme", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(typeof(MainActivity));
        }
    }
}