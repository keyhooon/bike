using bike.Services;
using Device;
using Device.Communication.Codec;
using Infrastructure;
using Prism.Mvvm;

namespace bike.ViewModels
{
    public class ConfigurationViewModel : ViewModel
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
        public CoreConfiguration Core => _servoDriveService.CoreConfiguration;
        public PedalConfiguration Pedal => _servoDriveService.PedalConfiguration;
        public ThrottleConfiguration Throttle => _servoDriveService.ThrottleConfiguration;

    }
}
