using DataModels;
using Device;
using Prism.Mvvm;

namespace bike.ViewModels.Settings
{
    public class ConfigurationViewModel : BindableBase
    {
        private readonly ServoDriveService _servoDriveService;
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel" /> class
        /// </summary>
        public ConfigurationViewModel(ServoDriveService servoDriveService)
        {
            _servoDriveService = servoDriveService;
        }


        #endregion

        public BatteryConfiguration Battery => _servoDriveService.BatteryConfiguration;
        public CoreVersion Core => _servoDriveService.CoreConfiguration;
        public PedalConfiguration Pedal => _servoDriveService.PedalConfiguration;
        public ServoConfiguration Servo => _servoDriveService.ServoConfiguration;
        public ThrottleConfiguration Throttle => _servoDriveService.ThrottleConfiguration;

    }
}
