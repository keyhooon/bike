using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;
namespace bike.Droid
{
    [Activity(Theme = "@style/MainTheme.Splash", 
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
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
    }

}

