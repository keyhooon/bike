using DataModels;
using Prism.Mvvm;
using Services;

namespace bike.ViewModels.Settings
{
    public class ConfigurationViewModel : BindableBase
    {
        private readonly BatteryManager batteryManager;
        private readonly CoreManager coreManager;
        private readonly PedalManager pedalManager;
        private readonly ThrottleManager throttleManager;
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel" /> class
        /// </summary>
        public ConfigurationViewModel(BatteryManager batteryManager, CoreManager coreManager, PedalManager pedalManager, ThrottleManager throttleManager)
        {
            this.batteryManager = batteryManager;
            this.coreManager = coreManager;
            this.pedalManager = pedalManager;
            this.throttleManager = throttleManager;
            batteryManager.PropertyChanged += (sender, e) => { if (e.PropertyName == nameof(batteryManager.BatteryConfiguration)) RaisePropertyChanged(nameof(BatteryConfiguration)); };
            coreManager.PropertyChanged += (sender, e) => { if (e.PropertyName == nameof(coreManager.CoreVersion)) RaisePropertyChanged(nameof(CoreVersion)); };
            coreManager.PropertyChanged += (sender, e) => { if (e.PropertyName == nameof(coreManager.CoreSituation)) RaisePropertyChanged(nameof(CoreSituation)); };
            pedalManager.PropertyChanged += (sender, e) => { if (e.PropertyName == nameof(pedalManager.PedalConfiguration)) RaisePropertyChanged(nameof(PedalConfiguration)); };
            throttleManager.PropertyChanged += (sender, e) => { if (e.PropertyName == nameof(throttleManager.ThrottleConfiguration)) RaisePropertyChanged(nameof(ThrottleConfiguration)); };
        }


        #endregion

        public BatteryConfiguration BatteryConfiguration => batteryManager.BatteryConfiguration;
        public CoreVersion CoreVersion => coreManager.CoreVersion;
        public CoreSituation CoreSituation => coreManager.CoreSituation;
        public PedalConfiguration PedalConfiguration => pedalManager.PedalConfiguration;
        public ThrottleConfiguration ThrottleConfiguration => throttleManager.ThrottleConfiguration;

    }
}
