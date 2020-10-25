using AiForms.Effects.Droid;
using AiForms.Renderers.Droid;
using Android.App;

using Android.Content.PM;
using Android.OS;
using Rg.Plugins.Popup;
using Shiny;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace bike.Droid
{
    [Activity(Label = "@string/ApplicationName", Theme = "@style/MainTheme.Main", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            LoadApplication(new App());

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            this.ShinyRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

}

