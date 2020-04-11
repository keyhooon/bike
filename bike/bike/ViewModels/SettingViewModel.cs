using DataModels;
using Device;
using Prism.Mvvm;
using Xamarin.Forms.Internals;

namespace bike.ViewModels.Settings
{
    /// <summary>
    /// ViewModel for Setting page 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class SettingViewModel : BindableBase
    {

        private readonly ServoDriveService _servoDriveService;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel" /> class
        /// </summary>
        public SettingViewModel(ServoDriveService servoDriveService) 
        {
            _servoDriveService = servoDriveService;
        }

        #endregion

        public LightSetting LightSetting => _servoDriveService.LightSetting; 
        public PedalSetting PedalSetting => _servoDriveService.PedalSetting;
    }
}
