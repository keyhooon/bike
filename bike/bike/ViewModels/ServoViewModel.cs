using bike.Services;
using Device;
using Device.Communication.Codec;
using Infrastructure;
using Prism.Mvvm;

namespace bike.ViewModels
{
    public class ServoViewModel : ViewModel
    {
        private readonly ServoDriveService _servoDriveService;
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel" /> class
        /// </summary>
        public ServoViewModel(ServoDriveService servoDriveService)
        {
            _servoDriveService = servoDriveService;
        }


        #endregion

        public BatteryConfiguration BatteryConfiguration => _servoDriveService.BatteryConfiguration;
        public BatteryOutput BatteryOutput => _servoDriveService.BatteryOutput;


        public CoreConfiguration CoreConfiguration => _servoDriveService.CoreConfiguration;
        public CoreSituation CoreSituation => _servoDriveService.Core;

        public Fault Fault => _servoDriveService.Fault;

        public PedalConfiguration PedalConfiguration => _servoDriveService.PedalConfiguration;
        public PedalSetting PedalSetting => _servoDriveService.PedalSetting;

        public ServoInput ServoInput => _servoDriveService.ServoInput;
        public ServoOutput ServoOutput => _servoDriveService.ServoOutput;

        public ThrottleConfiguration ThrottleConfiguration => _servoDriveService.ThrottleConfiguration;
        public ThrottleSetting ThrottleSetting => _servoDriveService.ThrottleSetting;

    }
}
