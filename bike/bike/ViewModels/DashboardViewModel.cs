using DataModels;
using Prism.Mvvm;

using System.Linq;

using Device;

namespace bike.ViewModels.Main
{
    public class DashboardViewModel : BindableBase
    {

        private readonly ServoDriveService _servoDriveService;

        public DashboardViewModel(ServoDriveService servoDriveService)
        {
            _servoDriveService = servoDriveService;
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
    }
}
