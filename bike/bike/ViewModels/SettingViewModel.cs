using DataModels;
using Device;
using Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;

namespace bike.ViewModels
{
    /// <summary>
    /// ViewModel for Setting page 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class SettingViewModel : ViewModel
    {

        private readonly ServoDriveService _servoDriveService;

        private string[] pedalAssistLevelTypeStringList;
        private string[] lightStringList;
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel" /> class
        /// </summary>
        public SettingViewModel(ServoDriveService servoDriveService) 
        {
            _servoDriveService = servoDriveService;
            PedalSetting.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(PedalSetting.AssistLevel))
                {
                    RaisePropertyChanged(nameof(PedalActive));
                    RaisePropertyChanged(nameof(PedalAssistActivityPercentText));
                    RaisePropertyChanged(nameof(PedalAssistActivityPercentIndex));
                }
            };
            ThrottleSetting.PropertyChanged += (sender, e) =>
            {
                RaisePropertyChanged(nameof(ThrottleSport));
                RaisePropertyChanged(nameof(ThrottleActive));
            };
            LightSetting.PropertyChanged += (sender, e) =>
            {
                RaisePropertyChanged(nameof(BackLightIndex));
                RaisePropertyChanged(nameof(FrontLightIndex));
                RaisePropertyChanged(nameof(BackLightText));
                RaisePropertyChanged(nameof(FrontLightText));
            };
        }
        #endregion

        public LightSetting LightSetting => _servoDriveService.LightSetting;
        public ThrottleSetting ThrottleSetting=> _servoDriveService.ThrottleSetting;
        public PedalSetting PedalSetting => _servoDriveService.PedalSetting;

        protected override async Task LoadAsync(INavigationParameters parameters, CancellationToken? cancellation)
        {
            pedalAssistLevelTypeStringList = typeof(PedalAssistLevelType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault())?.Description ?? x.Name).ToArray();
            lightStringList = new[] { "25 %", "50 %", "75 %", "100 %" };
            PedalAssistSensivityStringList = typeof(PedalActivationTimeType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault())?.Description ?? x.Name).ToList<string>();

            PedalActive = PedalSetting.AssistLevel != PedalAssistLevelType.Off;
            PedalAssistActivityPercentIndex = PedalAssistLevelType.Off - PedalSetting.AssistLevel;
            PedalAssistSensivityIndex = (int) PedalSetting.ActivationTime;
            ThrottleActive = ThrottleSetting.ActivityType != ThrottleActivityType.Off;
            ThrottleSport = ThrottleSetting.ActivityType == ThrottleActivityType.Sport;

            FrontLightIndex = (int) LightSetting.Light1;
            BackLightIndex = (int)LightSetting.Light2;

            await Task.CompletedTask;
        }
        private bool _pedalActive;
        public bool PedalActive
        {
            get => _pedalActive;
            set => SetProperty(ref _pedalActive, value);
        }


        public string PedalAssistActivityPercentText
        {
            get { return pedalAssistLevelTypeStringList[PedalAssistActivityPercentIndex]; }
        }

        private int _pedalAssistActivityPercentIndex;
        public int PedalAssistActivityPercentIndex
        {
            get => _pedalAssistActivityPercentIndex;
            set => SetProperty(ref _pedalAssistActivityPercentIndex, value);
        }


        private int _pedalAssistSensivityIndex;
        public int PedalAssistSensivityIndex
        {
            get => _pedalAssistSensivityIndex;
            set => SetProperty(ref _pedalAssistSensivityIndex, value);
        }

        private bool _throttleActive;
        public bool ThrottleActive
        {
            get => _throttleActive;
            set => SetProperty(ref _throttleActive, value);
        }

        private bool _throttleSport;
        public bool ThrottleSport
        {
            get => _throttleSport;
            set => SetProperty(ref _throttleSport, value);
        }

        private List<string> _pedalAssistSensivityStringList;
        public List<string> PedalAssistSensivityStringList { 
            get => _pedalAssistSensivityStringList; 
            set => SetProperty(ref _pedalAssistSensivityStringList, value); 
        }

        public string FrontLightText
        {
            get => lightStringList[FrontLightIndex]; 
        }

        private int _frontLightIndex;
        public int FrontLightIndex
        {
            get => _frontLightIndex;
            set => SetProperty(ref _frontLightIndex, value);
        }

        public string BackLightText
        {
            get => lightStringList[BackLightIndex]; 
        }

        private int _backLightIndex;
        public int BackLightIndex
        {
            get => _backLightIndex;
            set => SetProperty(ref _backLightIndex, value);
        }
    }
}
