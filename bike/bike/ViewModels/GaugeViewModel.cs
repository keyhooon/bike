using DataModels;
using Device;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace bike.ViewModels
{
    public class GaugeViewModel : BindableBase
    {
        private readonly ServoDriveService _servoDriveService;

        public GaugeViewModel(ServoDriveService servoDriveService)
        {

            _servoDriveService = servoDriveService;


            PedalAssistLevelList = typeof(PedalAssistLevelType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description).ToList();
            PedalAssistSensitivitiesList = typeof(PedalActivationTimeType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description).ToList();
            ThrottleModeList = typeof(ThrottleActivityType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description).ToList();

        }
        public List<string> PedalAssistLevelList { get; }

        public List<string> PedalAssistSensitivitiesList { get; }

        public List<string> ThrottleModeList { get; }

        public string PedalAssistLevelString { get; }

        public string PedalAssistSensitivitiesString { get; }

        public string ThrottleModeString { get; }

        public LightSetting LightSetting => _servoDriveService.LightSetting;
        public ThrottleSetting ThrottleSetting => _servoDriveService.ThrottleSetting;
        public PedalSetting PedalSetting => _servoDriveService.PedalSetting;

        public BatteryOutput Battery => _servoDriveService.Battery;
        public Fault Fault => _servoDriveService.Fault;
        public LightState Light => _servoDriveService.Light;
        public ServoOutput Servo => _servoDriveService.Servo;
    }
}
