using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battery.DataSettings
{
    public class BatteryConfigurationSetting : BindableBase
    {

        private static ISettings AppSettings => CrossSettings.Current;
        public double OverCurrent
        {
            get => AppSettings.GetValueOrDefault(nameof(OverCurrent), double.PositiveInfinity);
            set 
            { 
                AppSettings.AddOrUpdateValue(nameof(OverCurrent), value);
                RaisePropertyChanged();
            }
        }

        public double OverVoltage
        {
            get => AppSettings.GetValueOrDefault(nameof(OverVoltage), double.PositiveInfinity);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(OverVoltage), value);
                RaisePropertyChanged();
            }
        }

        public double UnderVoltage
        {
            get => AppSettings.GetValueOrDefault(nameof(UnderVoltage), double.NegativeInfinity);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(UnderVoltage), value);
                RaisePropertyChanged();
            }
        }

        public double NominalVoltage
        {
            get => AppSettings.GetValueOrDefault(nameof(NominalVoltage), double.Epsilon);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(NominalVoltage), value);
                RaisePropertyChanged();
            }
        }

        public double OverTemprature
        {
            get => AppSettings.GetValueOrDefault(nameof(OverTemprature), double.PositiveInfinity);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(OverTemprature), value);
                RaisePropertyChanged();
            }
        }
    }
}
