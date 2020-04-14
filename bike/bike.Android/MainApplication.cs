using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using bike.Shiny;
using Shiny;

namespace bike.Droid
{
    [Application(Label = "EV-Tech", 
        Icon = "@mipmap/ic_launcher")]
    public class MainApplication : Application
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
           AndroidShinyHost.Init(this, new Startup());
        }
         
    }
}