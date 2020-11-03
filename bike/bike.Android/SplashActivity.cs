using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using AiForms.Effects.Droid;
using AiForms.Renderers.Droid;
using Android.Content.PM;
using Rg.Plugins.Popup;
using Shiny;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
namespace bike.Droid
{
    [Activity(Theme = "@style/MainTheme.Splash", MainLauncher = false, NoHistory = true)]
    public class SplashActivity : FormsAppCompatActivity
    {
        static readonly string TAG = "X:" + typeof(SplashActivity).Name;
        Bundle _savedInstanceState;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            _savedInstanceState = savedInstanceState;
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
            
            // Create your application here
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(async () => {
                Log.Debug(TAG, "Performing some startup work that takes a bit of time.");
                Forms.Init(this, _savedInstanceState);
                Popup.Init(this, _savedInstanceState);

                SettingsViewInit.Init(); // need to write here
                Effects.Init(); //need to write here
                this.ShinyOnCreate();

                Log.Debug(TAG, "Startup work is finished - starting MainActivity.");
                StartActivity(new Intent(Application.ApplicationContext, typeof(MainActivity)));
            });
            startupWork.Start();
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() { }


    }
}