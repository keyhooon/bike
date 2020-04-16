using DataModels;
using Prism.Mvvm;

using System.Linq;

using Device;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Reflection;
using Prism.Commands;

namespace bike.ViewModels.Main
{
    public class DashboardViewModel : BindableBase
    {

        private readonly ServoDriveService _servoDriveService;

        public DashboardViewModel(ServoDriveService servoDriveService)
        {
            _servoDriveService = servoDriveService;
            PedalAssistLevelList = typeof(PedalAssistLevelType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description).ToList();
            PedalAssistSensitivitiesList = typeof(PedalActivationTimeType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description).ToList();
            ThrottleModeList = typeof(ThrottleActivityType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description).ToList();

            ThrottleSetting.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(ThrottleSetting.ActivityType))
                    ThrottleModeIsOpen = false;
            };
        }


        public BatteryConfiguration BatteryConfiguration => _servoDriveService.BatteryConfiguration;
        public CoreVersion CoreConfiguration => _servoDriveService.CoreConfiguration;
        public PedalConfiguration PedalConfiguration => _servoDriveService.PedalConfiguration;
        public ServoConfiguration ServoConfiguration => _servoDriveService.ServoConfiguration;
        public ThrottleConfiguration ThrottleConfiguration => _servoDriveService.ThrottleConfiguration;

        public LightSetting LightSetting => _servoDriveService.LightSetting;
        public ThrottleSetting ThrottleSetting => _servoDriveService.ThrottleSetting;
        public PedalSetting PedalSetting => _servoDriveService.PedalSetting;

        public BatteryOutput Battery => _servoDriveService.Battery;
        public Fault Fault => _servoDriveService.Fault;
        public LightState Light => _servoDriveService.Light;
        public ServoOutput Servo => _servoDriveService.Servo;
        public ServoInput ServoInput=> _servoDriveService.ServoInput;

        public List<string> PedalAssistLevelList { get; }

        public List<string> PedalAssistSensitivitiesList { get; }

        public List<string> ThrottleModeList{ get; }

        private bool _throttleModeIsOpen;
        public bool ThrottleModeIsOpen
        {
            get { return _throttleModeIsOpen; }
            set { SetProperty(ref _throttleModeIsOpen, value); }
        }
        private DelegateCommand _throttleModeOpenCommand;
        public DelegateCommand ThrottleModeOpenCommand =>
            _throttleModeOpenCommand ?? (_throttleModeOpenCommand = new DelegateCommand(ExecuteThrottleModeCommandOpen));

        void ExecuteThrottleModeCommandOpen()
        {
            ThrottleModeIsOpen = true;
        }
    }
}
