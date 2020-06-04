using Infrastructure.TypeConverters;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;
using System.ComponentModel;

namespace DataModels
{
    public class ThrottleSetting : BindableBase
    {

        private ThrottleActivityType _activityType;

        public ThrottleActivityType ActivityType
        {
            get => _activityType;
            set => SetProperty(ref _activityType, value);
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
