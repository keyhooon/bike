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
using bike.Droid.Services;
using bike.Shiny;
using Device.Communication.Codec;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SharpCommunication.Transport;
using Shiny;

namespace bike.Droid
{
    [Application(Label = "EV-Tech", 
        Icon = "@mipmap/ic_launcher")]
    public class MainApplication : ShinyAndroidApplication<Startup>
    {
        public MainApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }
        protected override void OnBuildApplication(IServiceCollection builder)
        {
            base.OnBuildApplication(builder);
            builder.AddSingleton<DataTransport<Packet>, BluetoothPacketDataTransport>();
        }
    }
}