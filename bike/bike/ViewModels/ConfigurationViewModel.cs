using bike.Services;
using Device;
using Device.Communication.Codec;
using Infrastructure;
using Prism.Mvvm;
using Prism.Navigation;

namespace bike.ViewModels
{
    public class ConfigurationViewModel : ViewModel, IMasterDetailPageOptions
    {
        private readonly ServoDriveService _servoDriveService;
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel" /> class
        /// </summary>
        public ConfigurationViewModel(ServoDriveService servoDriveService)
        {
            _servoDriveService = servoDriveService;
            _servoDriveService.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(ServoDriveService.BatteryConfiguration))
                    RaisePropertyChanged(nameof(Battery));
                else if (e.PropertyName == nameof(ServoDriveService.CoreConfiguration))
                    RaisePropertyChanged(nameof(Core));
                else if (e.PropertyName == nameof(ServoDriveService.PedalConfiguration))
                    RaisePropertyChanged(nameof(Pedal));
                else if (e.PropertyName == nameof(ServoDriveService.ThrottleConfiguration))
                    RaisePropertyChanged(nameof(Throttle));
            };
        }


        #endregion

        public BatteryConfiguration Battery => _servoDriveService.BatteryConfiguration;
        public CoreConfiguration Core => _servoDriveService.CoreConfiguration;
        public PedalConfiguration Pedal => _servoDriveService.PedalConfiguration;
        public ThrottleConfiguration Throttle => _servoDriveService.ThrottleConfiguration;

        public bool IsPresentedAfterNavigation => true;
    }
}
