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

        public BatteryConfiguration BatteryConfiguration => _servoDriveService.BatteryConfiguration;
        public CoreVersion CoreConfiguration => _servoDriveService.CoreConfiguration;
        public PedalConfiguration PedalConfiguration => _servoDriveService.PedalConfiguration;
        public ServoConfiguration ServoConfiguration => _servoDriveService.ServoConfiguration;
        public ThrottleConfiguration ThrottleConfiguration => _servoDriveService.ThrottleConfiguration;

    }
}
