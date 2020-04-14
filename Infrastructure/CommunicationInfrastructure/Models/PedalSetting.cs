
using Infrastructure.TypeConverters;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;
using System.ComponentModel;


namespace DataModels
{
    public class PedalSetting :BindableBase
    {

        private static ISettings AppSettings => CrossSettings.Current;

        public PedalAssistLevelType AssistLevel
        {
            get => (PedalAssistLevelType)AppSettings.GetValueOrDefault(nameof(AssistLevel), 4);
            set
            {
                if (value == AssistLevel)
                    return;
                AppSettings.AddOrUpdateValue(nameof(AssistLevel), (int)value);
                RaisePropertyChanged();
            }
        }

        public PedalActivationTimeType ActivationTime
        {
            get => (PedalActivationTimeType) AppSettings.GetValueOrDefault(nameof(ActivationTime), 3);
            set
            {
                if (value == ActivationTime)
                    return;
                AppSettings.AddOrUpdateValue(nameof(ActivationTime), (int)value);
                RaisePropertyChanged();
            }

        }
     
    }
    [Xamarin.Forms.TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum PedalAssistLevelType : int
    {
        [Description("87.5%")]
        EightySevenPointFive = 0,
        [Description("75  %")]
        SeventyFive = 1,
        [Description("62.5%")]
        SixtyTwoPointFive = 2,
        [Description("50  %")]
        Fifty = 3,
        [Description("37.5%")]
        ThirtySevenPointFive = 4,
        [Description("31.2%")]
        ThrityOnePointTwentyFive = 5,
        [Description("25  %")]
        TowentyFive = 6,
        [Description("OFF  ")]
        Off = 7,
    }
    [Xamarin.Forms.TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum PedalActivationTimeType : int
    {
        [Description("EXTRA")]
        ExteraSensitive = 0,
        [Description("HIGH ")]
        HighSensitive = 1,
        [Description("NORM ")]
        NormalSensitive = 2,
        [Description("LOW  ")]
        LowSensitive = 3,
    }
}
