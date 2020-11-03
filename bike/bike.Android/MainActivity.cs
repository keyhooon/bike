using System.Threading.Tasks;
using AiForms.Effects.Droid;
using AiForms.Renderers.Droid;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Rg.Plugins.Popup;
using Shiny;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace bike.Droid
{
    [Activity(MainLauncher = true,
        Theme = "@style/MainTheme.Splash", 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        private PowerManager.WakeLock wakeLock;
        protected override void OnResume()
        {
            base.OnResume();
            var powerManager = (PowerManager)this.GetSystemService(Context.PowerService);
            wakeLock = powerManager.NewWakeLock(WakeLockFlags.Full, "My Lock");
            wakeLock.Acquire();

        }

        protected override void OnPause()
        {
            base.OnPause();
            wakeLock.Release();
        }

        protected override void OnCreate(Bundle bundle)
        {

            // Name of the MainActivity theme you had there before.
            // Or you can use global::Android.Resource.Style.ThemeHoloLight
            base.SetTheme(Resource.Style.MainTheme);
            Window.RequestFeature(Android.Views.WindowFeatures.ActionBar);
            base.OnCreate(bundle);
            Task.Run(() =>
            {

                Popup.Init(this, bundle);

                SettingsViewInit.Init(); // need to write here
                Effects.Init(); //need to write here
                this.ShinyOnCreate();
            });
                Forms.Init(this, bundle);


                LoadApplication(new App());

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            this.ShinyRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

}

