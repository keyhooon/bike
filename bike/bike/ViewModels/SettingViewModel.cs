using Device;
using Device.Communication.Codec;
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
using bike.Services;
using Acr.UserDialogs.Forms;

namespace bike.ViewModels
{
    /// <summary>
    /// ViewModel for Setting page 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class SettingViewModel : ViewModel
    {

        private readonly ServoDriveService _servoDriveService;
        private readonly IUserDialogs dialogs;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel" /> class
        /// </summary>
        public SettingViewModel(ServoDriveService servoDriveService, IUserDialogs dialogs)
        {
            _servoDriveService = servoDriveService;
            this.dialogs = dialogs;
        }
        #endregion
        private List<string> pedalAssistLevelActivityStringList;
        private List<string> pedalAssistSensivityStringList;
        private List<string> lightStringList;

        protected override async Task LoadDataAsync(INavigationParameters parameters, CancellationToken? cancellation)
        {
            PedalAssistLevelActivityStringList = typeof(PedalSetting.PedalAssistLevelType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault())?.Description ?? x.Name).ToList();
            LightStringList = new List<string>(new[] { "25 %", "50 %", "75 %", "100 %" });
            PedalAssistSensivityStringList = typeof(PedalSetting.PedalActivationTimeType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault())?.Description ?? x.Name).ToList();

            _pedalAssistActivityPercentIndex = _servoDriveService.PedalSetting.AssistLevel == PedalSetting.PedalAssistLevelType.Off ? (int)_servoDriveService.PedalSetting.AssistLevel + 1 : (int)_servoDriveService.PedalSetting.AssistLevel;
            _pedalActive = _servoDriveService.PedalSetting.AssistLevel != PedalSetting.PedalAssistLevelType.Off;
            _pedalAssistSensivityString = PedalAssistSensivityStringList[(int)_servoDriveService.PedalSetting.ActivationTime];
            RaisePropertyChanged(nameof(PedalAssistActivityPercentIndex));
            RaisePropertyChanged(nameof(PedalAssistActivityPercentText));
            RaisePropertyChanged(nameof(PedalActive));
            RaisePropertyChanged(nameof(PedalAssistSensivityString));

            _throttleSport = _servoDriveService.ThrottleSetting.ActivityType == ThrottleSetting.ThrottleActivityType.Sport;
            _throttleActive = _servoDriveService.ThrottleSetting.ActivityType != ThrottleSetting.ThrottleActivityType.Off;
            RaisePropertyChanged(nameof(ThrottleSport));
            RaisePropertyChanged(nameof(ThrottleActive));

            _frontLightIndex = (int)_servoDriveService.LightSetting.Light1;
            _backLightIndex = (int)_servoDriveService.LightSetting.Light2;
            RaisePropertyChanged(nameof(FrontLightText));
            RaisePropertyChanged(nameof(BackLightText)); 
            RaisePropertyChanged(nameof(FrontLightIndex));
            RaisePropertyChanged(nameof(BackLightIndex));

            await Task.CompletedTask;
        }



        public List<string> PedalAssistLevelActivityStringList { get => pedalAssistLevelActivityStringList; private set => SetProperty(ref pedalAssistLevelActivityStringList , value); }
        public List<string> PedalAssistSensivityStringList { get => pedalAssistSensivityStringList; private set => SetProperty(ref pedalAssistSensivityStringList, value); }
        public List<string> LightStringList { get => lightStringList; private set => SetProperty(ref lightStringList , value); }

        private bool _pedalActive;
        public bool PedalActive
        {
            get => _pedalActive;
            set => SetProperty(ref _pedalActive, value, () =>
            {
                _servoDriveService.PedalSetting = new PedalSetting
                {
                    AssistLevel = PedalActive ? 
                    (PedalSetting.PedalAssistLevelType)PedalAssistActivityPercentIndex : 
                    PedalSetting.PedalAssistLevelType.Off,
                    ActivationTime = (PedalSetting.PedalActivationTimeType)PedalAssistSensivityStringList.IndexOf(PedalAssistSensivityString)
                };
                RaisePropertyChanged(nameof(PedalAssistActivityPercentText));
            });
        }

        public string PedalAssistActivityPercentText => PedalAssistLevelActivityStringList?[PedalAssistActivityPercentIndex];


        private int _pedalAssistActivityPercentIndex;
        public int PedalAssistActivityPercentIndex
        {
            get => _pedalAssistActivityPercentIndex;
            set => SetProperty(ref _pedalAssistActivityPercentIndex, value, () =>
            {
                _servoDriveService.PedalSetting = new PedalSetting
                {
                    AssistLevel = PedalActive ? 
                    (PedalSetting.PedalAssistLevelType)PedalAssistActivityPercentIndex : 
                    PedalSetting.PedalAssistLevelType.Off,
                    ActivationTime = (PedalSetting.PedalActivationTimeType)PedalAssistSensivityStringList.IndexOf(PedalAssistSensivityString)
                };
                RaisePropertyChanged(nameof(PedalAssistActivityPercentText));
            });
        }


        private string _pedalAssistSensivityString;
        public string PedalAssistSensivityString
        {
            get => _pedalAssistSensivityString;
            set => SetProperty(ref _pedalAssistSensivityString, value, () => 
            {
                _servoDriveService.PedalSetting = new PedalSetting
                {
                    AssistLevel = PedalActive ? 
                    (PedalSetting.PedalAssistLevelType)PedalAssistActivityPercentIndex : 
                    PedalSetting.PedalAssistLevelType.Off,
                    ActivationTime = (PedalSetting.PedalActivationTimeType)PedalAssistSensivityStringList.IndexOf(PedalAssistSensivityString)
                };
            }
            );
        }

        private bool _throttleActive;
        public bool ThrottleActive
        {
            get => _throttleActive;
            set => SetProperty(ref _throttleActive, value, () =>
            {
                _servoDriveService.ThrottleSetting = new ThrottleSetting
                {
                    ActivityType = 
                    ThrottleActive ? 
                    ThrottleSport ? 
                    ThrottleSetting.ThrottleActivityType.Sport : 
                    ThrottleSetting.ThrottleActivityType.Normal : 
                    ThrottleSetting.ThrottleActivityType.Off
                };
            });
        }

        private bool _throttleSport;
        public bool ThrottleSport
        {
            get => _throttleSport;
            set => SetProperty(ref _throttleSport, value, () =>
            {
                _servoDriveService.ThrottleSetting = new ThrottleSetting
                {
                    ActivityType = 
                    ThrottleActive ? 
                    ThrottleSport ? 
                    ThrottleSetting.ThrottleActivityType.Sport : 
                    ThrottleSetting.ThrottleActivityType.Normal : 
                    ThrottleSetting.ThrottleActivityType.Off
                };
            });
        }

        public string FrontLightText => LightStringList?[FrontLightIndex];

        private int _frontLightIndex;
        public int FrontLightIndex
        {
            get => _frontLightIndex;
            set => SetProperty(ref _frontLightIndex, value, () =>
            {
                _servoDriveService.LightSetting = new LightSetting
                {
                    Light1 = (LightSetting.LightVolume)FrontLightIndex,
                    Light2 = (LightSetting.LightVolume)BackLightIndex,
                };
                RaisePropertyChanged(nameof(FrontLightText));
            });
        }

        public string BackLightText => LightStringList?[BackLightIndex];

        private int _backLightIndex;
        public int BackLightIndex
        {
            get => _backLightIndex;
            set => SetProperty(ref _backLightIndex, value, () =>
            {
                _servoDriveService.LightSetting = new LightSetting
                {
                    Light1 = (LightSetting.LightVolume)FrontLightIndex,
                    Light2 = (LightSetting.LightVolume)BackLightIndex,
                };
                RaisePropertyChanged(nameof(BackLightText));
            });
        }

        private DelegateCommand _resetCommand;

        public DelegateCommand RestCommand => _resetCommand ?? (_resetCommand = new DelegateCommand(async() =>
        {
            var confirm = await dialogs.Confirm("Reset Settings?");
            if (confirm)
            {
                _servoDriveService.LightSetting = new LightSetting();
                _servoDriveService.PedalSetting = new PedalSetting();
                _servoDriveService.ThrottleSetting = new ThrottleSetting();
                Initialize(null);
            }

        }));
    }
}
