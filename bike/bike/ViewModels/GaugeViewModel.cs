using Device;
using Device.Communication.Codec;
using Prism.Mvvm;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace bike.ViewModels
{
    public class GaugeViewModel : BindableBase
    {
        private readonly ServoDriveService _servoDriveService;
        private string pedalAssistLevelString;
        private string pedalAssistSensitivitiesString;
        private string throttleModeString;

        public GaugeViewModel(ServoDriveService servoDriveService)
        {
            _servoDriveService = servoDriveService;

            PedalAssistLevelList = typeof(PedalSetting.PedalAssistLevelType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description).ToList();
            PedalAssistSensitivitiesList = typeof(PedalSetting.PedalActivationTimeType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description).ToList();
            ThrottleModeList = typeof(ThrottleSetting.ThrottleActivityType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description).ToList();
            pedalAssistLevelString = PedalAssistLevelList[(int)servoDriveService.PedalSetting.AssistLevel];
            pedalAssistSensitivitiesString = PedalAssistSensitivitiesList[(int)servoDriveService.PedalSetting.ActivationTime];
            throttleModeString = ThrottleModeList[(int)servoDriveService.ThrottleSetting.ActivityType];

        }
        public List<string> PedalAssistLevelList { get; }

        public List<string> PedalAssistSensitivitiesList { get; }

        public List<string> ThrottleModeList { get; }

        public string PedalAssistLevelString { get => pedalAssistLevelString; set => SetProperty(ref pedalAssistLevelString, value,()=>_servoDriveService.PedalSetting.AssistLevel = (PedalSetting.PedalAssistLevelType)PedalAssistLevelList.IndexOf(value)); }

        public string PedalAssistSensitivitiesString { get => pedalAssistSensitivitiesString; set => SetProperty(ref pedalAssistSensitivitiesString, value, () => _servoDriveService.PedalSetting.ActivationTime = (PedalSetting.PedalActivationTimeType)PedalAssistSensitivitiesList.IndexOf(value)); }

        public string ThrottleModeString { get => throttleModeString; set => SetProperty(ref throttleModeString, value, () => _servoDriveService.ThrottleSetting.ActivityType = (ThrottleSetting.ThrottleActivityType)ThrottleModeList.IndexOf(value)); }

        public LightSetting LightSetting => _servoDriveService.LightSetting;
        public ThrottleSetting ThrottleSetting => _servoDriveService.ThrottleSetting;
        public PedalSetting PedalSetting => _servoDriveService.PedalSetting;

        public BatteryOutput Battery => _servoDriveService.BatteryOutput;
        public Fault Fault => _servoDriveService.Fault;
        public LightState Light => _servoDriveService.LightState;
        public ServoOutput Servo => _servoDriveService.ServoOutput;
    }
}
