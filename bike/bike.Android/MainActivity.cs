using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Prism;
using Prism.Ioc;
using Shiny;
using Xamarin.Forms.Platform.Android;

namespace bike.Droid
{
    [Activity(Theme = "@style/MainTheme.Splash", 
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {


            // Name of the MainActivity theme you had there before.
            // Or you can use global::Android.Resource.Style.ThemeHoloLight
            base.SetTheme(Resource.Style.MainTheme);
            Window.RequestFeature(Android.Views.WindowFeatures.ActionBar);
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            AiForms.Renderers.Droid.SettingsViewInit.Init(); // need to write here
            AiForms.Effects.Droid.Effects.Init(); //need to write here
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            this.ShinyRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

}

