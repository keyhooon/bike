using bike.Services;

using Device.Communication.Codec;
using Infrastructure;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace bike.ViewModels
{
    public class GaugeViewModel : ViewModel
    {
        private readonly ServoDriveService _servoDriveService;

        private List<string> pedalAssistLevelList;
        private List<string> pedalAssistSensitivitiesList;
        private List<string> throttleModeList;
        private int _selectedPedalAssistLevel;
        private int _selectedpedalAssistSensitivities;
        private int _selectedthrottleMode;
        private BatteryOutput _batteryOutput;
        private Fault _fault;
        private LightState _light;
        private ServoOutput _servo;

        public GaugeViewModel(ServoDriveService servoDriveService)
        {
            _servoDriveService = servoDriveService;
            _servoDriveService.PropertyChanged += (sender, e) =>
            {
                RaisePropertyChanged(e.PropertyName);
            };
        }
        protected override async Task InitAsync(INavigationParameters parameters, CancellationToken? cancellation = null)
        {
            PedalAssistLevelList = typeof(PedalSetting.PedalAssistLevelType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description).ToList();
            PedalAssistSensitivitiesList = typeof(PedalSetting.PedalActivationTimeType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description).ToList();
            ThrottleModeList = typeof(ThrottleSetting.ThrottleActivityType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description).ToList();
            _selectedPedalAssistLevel = (int)ServoDriveService.PedalSetting.AssistLevel;
            _selectedpedalAssistSensitivities = (int)ServoDriveService.PedalSetting.ActivationTime;
            _selectedthrottleMode = (int)ServoDriveService.ThrottleSetting.ActivityType;
            RaisePropertyChanged(nameof(SelectedPedalAssistLevel));
            RaisePropertyChanged(nameof(SelectedPedalAssistSensitivities));
            RaisePropertyChanged(nameof(SelectedThrottleMode));
            await Task.CompletedTask;
        }
        public List<string> PedalAssistLevelList { get => pedalAssistLevelList; private set => SetProperty(ref pedalAssistLevelList, value); }

        public List<string> PedalAssistSensitivitiesList { get => pedalAssistSensitivitiesList; private set => SetProperty(ref pedalAssistSensitivitiesList, value); }

        public List<string> ThrottleModeList { get => throttleModeList; private set => SetProperty(ref throttleModeList, value); }

        public int SelectedPedalAssistLevel
        {
            get => _selectedPedalAssistLevel; set => SetProperty(ref _selectedPedalAssistLevel, value, () =>
            {
                if (value == -1)
                {
                    _selectedPedalAssistLevel = (int)ServoDriveService.PedalSetting.AssistLevel;
                    return;
                }
                ServoDriveService.PedalSetting =
                new PedalSetting
                {
                    ActivationTime = (PedalSetting.PedalActivationTimeType)_selectedpedalAssistSensitivities,
                    AssistLevel = (PedalSetting.PedalAssistLevelType)_selectedPedalAssistLevel
                };
            });
        }

        public int SelectedPedalAssistSensitivities
        {
            get => _selectedpedalAssistSensitivities; set => SetProperty(ref _selectedpedalAssistSensitivities, value, () =>
            {
                if (value == -1)
                {
                    _selectedpedalAssistSensitivities = (int)ServoDriveService.PedalSetting.ActivationTime;
                    return;
                }
                ServoDriveService.PedalSetting =
                new PedalSetting
                {
                    ActivationTime = (PedalSetting.PedalActivationTimeType)_selectedpedalAssistSensitivities,
                    AssistLevel = (PedalSetting.PedalAssistLevelType)_selectedPedalAssistLevel
                };
            });
        }

        public int SelectedThrottleMode
        {
            get => _selectedthrottleMode; set => SetProperty(ref _selectedthrottleMode, value, () =>
            {
                if (value == -1)
                {
                    _selectedthrottleMode = (int)ServoDriveService.ThrottleSetting.ActivityType;
                    return;
                }
                ServoDriveService.ThrottleSetting =
                new ThrottleSetting
                {
                    ActivityType = (ThrottleSetting.ThrottleActivityType)_selectedthrottleMode
                };
            });
        }

        public BatteryOutput Battery { get => _batteryOutput; set => SetProperty(ref _batteryOutput, value, () =>
        { 
        
        }); }
        public Fault Fault { get => _fault; set => SetProperty(ref _fault, value, () => 
        { 
        
        }); }
        public LightState Light { get => _light; set => SetProperty(ref _light, value, () =>
        { 
        
        }); }
        public ServoOutput Servo { get => _servo; set => SetProperty(ref _servo, value, () => 
        { 
        
        }); }

        public ServoDriveService ServoDriveService => _servoDriveService;
    }
}
