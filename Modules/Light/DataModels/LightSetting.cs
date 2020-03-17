using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace DataModels
{
    public class LightSetting : BindableBase
    {

        private static ISettings AppSettings => CrossSettings.Current;

        public LightVolume Light1
        {
            get => (LightVolume)AppSettings.GetValueOrDefault(nameof(Light1), (byte)LightVolume.High);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Light1), (byte)value);
                RaisePropertyChanged();
            }
        }

        public LightVolume Light2
        {
            get => (LightVolume)AppSettings.GetValueOrDefault(nameof(Light2), (byte)LightVolume.High);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Light2), (byte)value);
                RaisePropertyChanged();
            }
        }

        public LightVolume Light3
        {
            get => (LightVolume)AppSettings.GetValueOrDefault(nameof(Light3), (byte)LightVolume.High);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Light3), (byte)value);
                RaisePropertyChanged();
            }
        }

        public LightVolume Light4
        {
            get => (LightVolume)AppSettings.GetValueOrDefault(nameof(Light4), (byte)LightVolume.High);
            set
            {
                AppSettings.AddOrUpdateValue(nameof(Light4), (byte)value);
                RaisePropertyChanged();
            }
        }
    }

    public enum LightVolume : byte
    {
        High = 3,
        Normal = 2,
        Low = 1,
        ExtraLow = 0
    }
}
