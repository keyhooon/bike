﻿using bike.Services;
using Device;
using Device.Communication.Codec;
using Infrastructure;
using Prism.Mvvm;
using Prism.Navigation;

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
            _servoDriveService.PropertyChanged += (sender, e) => RaisePropertyChanged(nameof(e.PropertyName));
        }


        #endregion

        public BatteryConfiguration BatteryConfiguration => _servoDriveService.BatteryConfiguration;
        public CoreConfiguration CoreConfiguration => _servoDriveService.CoreConfiguration;
        public PedalConfiguration PedalConfiguration => _servoDriveService.PedalConfiguration;
        public ThrottleConfiguration ThrottleConfiguration => _servoDriveService.ThrottleConfiguration;
    }
}
