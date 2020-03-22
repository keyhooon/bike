using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;

namespace Servo.DataModels
{
    public class ServoConfiguration : BindableBase
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public double WheelRadius
        {
            get => AppSettings.GetValueOrDefault(nameof(WheelRadius), 20.0);
            set
            {
                if (value == WheelRadius)
                    return;
                AppSettings.AddOrUpdateValue(nameof(WheelRadius), (byte)value);
                RaisePropertyChanged();
            }
        }
    }
}
