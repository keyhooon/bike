using Core.TypeConverters;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;
using System.ComponentModel;

namespace DataModels
{
    public class ThrottleSetting : BindableBase
    {

        private static ISettings AppSettings => CrossSettings.Current;

        public ThrottleActivityType ThrottleActivity
        {
            get => (ThrottleActivityType)AppSettings.GetValueOrDefault(nameof(ThrottleActivity), 3);
            set
            {
                if (value == ThrottleActivity)
                    return;
                AppSettings.AddOrUpdateValue(nameof(ThrottleActivity), (int)value);
                RaisePropertyChanged();
            }

        }
    }
    [Xamarin.Forms.TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum ThrottleActivityType : int
    {
        [Description("NORMAL")]
        Normal = 0,
        [Description("SPORT")]
        Sport = 1,
        [Description("OFF")]
        Off = 2,
    }
}
