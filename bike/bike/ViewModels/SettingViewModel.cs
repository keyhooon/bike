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
        public ThrottleSetting ThrottleSetting=> _servoDriveService.ThrottleSetting;
        public PedalSetting PedalSetting => _servoDriveService.PedalSetting;

        protected override async Task LoadAsync(INavigationParameters parameters, CancellationToken? cancellation)
        {

            PedalAssistLevelActivityStringList = typeof(PedalAssistLevelType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault())?.Description ?? x.Name).Reverse().ToList();
            LightStringList = new List<string>( new[] { "25 %", "50 %", "75 %", "100 %" });
            PedalAssistSensivityStringList = typeof(PedalActivationTimeType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault())?.Description ?? x.Name).ToList();

            PedalActive = PedalSetting.AssistLevel != PedalAssistLevelType.Off;
            PedalAssistActivityPercentIndex = PedalAssistLevelType.Off - PedalSetting.AssistLevel;
            PedalAssistSensivityString = PedalAssistSensivityStringList[(int) PedalSetting.ActivationTime];
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

        private List<string> _pedalAssistLevelActivityStringList;
        public List<string> PedalAssistLevelActivityStringList
        {
            get => _pedalAssistLevelActivityStringList;
            set => SetProperty(ref _pedalAssistLevelActivityStringList, value);
        }

        private string _pedalAssistActivityPercentText;
        public string PedalAssistActivityPercentText
        {
            get => _pedalAssistActivityPercentText;
            private set => SetProperty(ref _pedalAssistActivityPercentText, value);
        }

        private int _pedalAssistActivityPercentIndex;
        public int PedalAssistActivityPercentIndex
        {
            get => _pedalAssistActivityPercentIndex;
            set => SetProperty(ref _pedalAssistActivityPercentIndex, value, PedalAssistActivityPercentText = PedalAssistLevelActivityStringList[value]);
        }

        private List<string> _pedalAssistSensivityStringList;
        public List<string> PedalAssistSensivityStringList
        {
            get => _pedalAssistSensivityStringList;
            set => SetProperty(ref _pedalAssistSensivityStringList, value);
        }

        private string _pedalAssistSensivityString;
        public string PedalAssistSensivityString
        {
            get => _pedalAssistSensivityString;
            set => SetProperty(ref _pedalAssistSensivityString, value);
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

        private List<string> _lightStringList;
        public List<string> LightStringList
        {
            get => _lightStringList;
            set => SetProperty(ref _lightStringList, value);
        }

        private string _frontLightText;
        public string FrontLightText
        {
            get => _frontLightText;
            private set => SetProperty(ref _frontLightText, value);
        }

        private int _frontLightIndex;
        public int FrontLightIndex
        {
            get => _frontLightIndex;
            set => SetProperty(ref _frontLightIndex, value, FrontLightText = LightStringList[value]);
        }

        private string _backLightText;
        public string BackLightText
        {
            get => _backLightText;
            private set => SetProperty(ref _backLightText, value);
        }

        private int _backLightIndex;
        public int BackLightIndex
        {
            get => _backLightIndex;
            set => SetProperty(ref _backLightIndex, value, BackLightText = LightStringList[value]);
        }
    }
}
