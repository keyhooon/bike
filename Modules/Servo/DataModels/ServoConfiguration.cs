using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Servo.DataModels
{
    public class ServoConfiguration : BindableBase
    {
        private static ISettings AppSettings => CrossSettings.Current;

        private double _wheelRadius;
        public double WheelRadius
        {
            get => AppSettings.GetValueOrDefault(nameof(WheelRadius), 20.0);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(WheelRadius), (byte)value);
                RaisePropertyChanged();
            }
        }
    }
}
