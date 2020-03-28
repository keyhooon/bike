using DataModels;
using Prism.Mvvm;
using Services;
using Xamarin.Forms.Internals;

namespace bike.ViewModels.Settings
{
    /// <summary>
    /// ViewModel for Setting page 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class SettingViewModel : BindableBase
    {

        private readonly CoreManager coreManager;
        private readonly LightManager lightManager;
        private readonly PedalManager pedalManager;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel" /> class
        /// </summary>
        public SettingViewModel(CoreManager coreManager, LightManager lightManager, PedalManager pedalManager) 
        {

            this.coreManager = coreManager;
            this.lightManager = lightManager;
            this.pedalManager = pedalManager;
            coreManager.PropertyChanged += (sender, e) => { if (e.PropertyName == nameof(coreManager.CoreVersion)) RaisePropertyChanged(nameof(CoreVersion)); };
            lightManager.PropertyChanged += (sender, e) => { if (e.PropertyName == nameof(lightManager.LightSetting)) RaisePropertyChanged(nameof(LightSetting)); };
            pedalManager.PropertyChanged += (sender, e) => { if (e.PropertyName == nameof(pedalManager.PedalSetting)) RaisePropertyChanged(nameof(PedalSetting)); };

        }

        #endregion

        public CoreVersion CoreVersion => coreManager.CoreVersion;
        public LightSetting LightSetting => lightManager.LightSetting; 
        public PedalSetting PedalSetting => pedalManager.PedalSetting;
    }
}
