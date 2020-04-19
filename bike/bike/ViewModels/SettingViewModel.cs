using DataModels;
using Device;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms.Internals;

namespace bike.ViewModels
{
    /// <summary>
    /// ViewModel for Setting page 
    /// </summary> 
    [Preserve(AllMembers = true)]
    public class SettingViewModel : BindableBase
    {

        private readonly ServoDriveService _servoDriveService;

        private string[] pedalAssistLevelTypeStringList;
        private string[] lightStringList;
        List<string> _pedalAssistSensivityStringList;
        public List<string> PedalAssistSensivityStringList { get => _pedalAssistSensivityStringList; set { SetProperty(ref _pedalAssistSensivityStringList, value); } }
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel" /> class
        /// </summary>
        public SettingViewModel(ServoDriveService servoDriveService) 
        {
            _servoDriveService = servoDriveService;
            pedalAssistLevelTypeStringList = typeof(PedalAssistLevelType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault())?.Description ?? x.Name).ToArray();
            lightStringList = new[] { "25 %", "50 %", "75 %", "100 %" };
            PedalAssistSensivityStringList = typeof(PedalActivationTimeType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault())?.Description ?? x.Name).ToList<string>();
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



        public bool PedalActive
        {
            get { return PedalSetting.AssistLevel != PedalAssistLevelType.Off; }
            set { PedalSetting.AssistLevel = (value?PedalAssistLevelType.ThirtySevenPointFive:PedalAssistLevelType.Off); }
        }

        public string PedalAssistActivityPercentText
        {
            get { return pedalAssistLevelTypeStringList[(int)PedalSetting.AssistLevel]; }
        }

        public int PedalAssistActivityPercentIndex
        {
            get { return (int)PedalAssistLevelType.Off - (int)PedalSetting.AssistLevel; }
            set { if (!PedalActive) return; PedalSetting.AssistLevel = (PedalAssistLevelType.Off - value); }
        }

        public int PedalAssistSensivityIndex
        {
            get { return (int) PedalSetting.ActivationTime; }
            set { if (!PedalActive) return; PedalSetting.ActivationTime = (PedalActivationTimeType)value; }
        }

        public bool ThrottleActive
        {
            get { return ThrottleSetting.ActivityType != ThrottleActivityType.Off; }
            set { ThrottleSetting.ActivityType = (value ? ThrottleActivityType.Normal : ThrottleActivityType.Off); }
        }

        public bool ThrottleSport
        {
            get { return ThrottleSetting.ActivityType==ThrottleActivityType.Sport; }
            set { if (!ThrottleActive) return; ThrottleSetting.ActivityType = value ? ThrottleActivityType.Sport : ThrottleActivityType.Off; }
        }

        public string FrontLightText
        {
            get { return lightStringList[(int)LightSetting.Light1]; }
        }

        public int FrontLightIndex
        {
            get { return (int)LightSetting.Light1 ; }
            set { LightSetting.Light1 = (LightVolume)value; }
        }

        public string BackLightText
        {
            get { return lightStringList[(int)LightSetting.Light2]; }
        }

        public int BackLightIndex
        {
            get { return (int)LightSetting.Light2; }
            set { LightSetting.Light2 = (LightVolume)value; }
        }
    }
}
