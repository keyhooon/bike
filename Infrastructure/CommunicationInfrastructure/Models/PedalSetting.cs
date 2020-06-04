
using Infrastructure.TypeConverters;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DataModels
{
    public class PedalSetting :BindableBase
    {

        private PedalAssistLevelType _assistLevel;
        [Display(Name = "Level of Assist", Prompt = "Enter Level of Assist for Pedal", Description = "Level of Assist; If you want higher speed when Pedaling set High level for Pedal Assist")]
        public PedalAssistLevelType AssistLevel
        {
            get => _assistLevel;
            set => SetProperty(ref _assistLevel, value);
        }


        public PedalActivationTimeType _activationTime;

        [Display(Name = "Sensivity of Assit", Prompt = "Enter Sensivity of Assist for Pedal", Description = "Sensivity of Pedal Assist.")]
        public PedalActivationTimeType ActivationTime
        {
            get => _activationTime;
            set => SetProperty(ref _activationTime, value);
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
