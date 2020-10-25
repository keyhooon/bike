using Device;
using Infrastructure;
using Prism.Commands;
using bike.Services;

using Prism.Navigation;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs.Forms;
using Prism.Services;
using System.Collections.Generic;
using Device.Communication.Codec;
using System.Reflection;
using System.Linq;
using System.ComponentModel;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Prism.Ioc;

namespace bike.ViewModels
{
    public class MainViewModel : ViewModel, IInitializeAsync
    {
        private readonly INavigationService navigationService;
        private readonly IUserDialogs dialogs;
        private readonly IContainerRegistry containerRegistry;
        private bool isNavigateOnProgress;
        private bool _masterIsPresent;

        public MainViewModel(INavigationService navigationService, ServoDriveService servoDriveService, IUserDialogs dialogs, IContainerRegistry containerRegistry)
        {
            this.navigationService = navigationService;
            this.dialogs = dialogs;
            this.containerRegistry = containerRegistry;
            ServoDriveService = servoDriveService;

            Task.Run(async() => {
                await Task.Delay(300);

            });
        }

        DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand => _navigateCommand ??= new DelegateCommand<string>(async (x) =>
        {
           
            IsNavigateOnProgress = true;
            //IsBusy = true;
            Application.Current.Dispatcher.BeginInvokeOnMainThread(
               async () => {await navigationService.NavigateAsync(x); });
            await Task.Delay(1000);
            //IsBusy = false;
            IsPresented = false;
            IsNavigateOnProgress = false;

        });

        DelegateCommand _openDrawerCommand;
        public DelegateCommand OpenDrawerCommand => _openDrawerCommand ??= new DelegateCommand(() =>
        {
            IsPresented = true;
        });

        public bool IsConnected
        {
            get => ServoDriveService.IsOpen;
        }


        public bool IsPresented
        {
            get => _masterIsPresent; set => SetProperty(ref _masterIsPresent, value);
        }
        public bool IsNavigateOnProgress
        {
            get => isNavigateOnProgress; set => SetProperty(ref isNavigateOnProgress, value);
        }
        public Collection<NavigationItem> NavigationItems { get; protected set; }

        private List<string> pedalAssistLevelList;
        private List<string> pedalAssistSensitivitiesList;
        private List<string> throttleModeList;
        private int _selectedPedalAssistLevel;
        private int _selectedpedalAssistSensitivities;
        private int _selectedthrottleMode;


        public async Task InitializeAsync(INavigationParameters parameters)
        {
            ServoDriveService.PropertyChanged += (sender, e) =>
            {
                RaisePropertyChanged(e.PropertyName);
                if (e.PropertyName == nameof(ServoDriveService.PedalSetting))
                {
                    _selectedPedalAssistLevel = (int)ServoDriveService.PedalSetting.AssistLevel;
                    _selectedpedalAssistSensitivities = (int)ServoDriveService.PedalSetting.ActivationTime;
                    RaisePropertyChanged(nameof(SelectedPedalAssistLevel));
                    RaisePropertyChanged(nameof(SelectedPedalAssistSensitivities));
                }
                if (e.PropertyName == nameof(ServoDriveService.ThrottleSetting))
                {
                    _selectedthrottleMode = (int)ServoDriveService.ThrottleSetting.ActivityType;
                    RaisePropertyChanged(nameof(SelectedThrottleMode));
                }
            };

            ServoDriveService.IsOpenChanged += (sender, e) =>
            {
                RaisePropertyChanged(nameof(IsConnected));
            };


            PedalAssistLevelList = typeof(PedalSetting.PedalAssistLevelType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description).ToList();
            PedalAssistSensitivitiesList = typeof(PedalSetting.PedalActivationTimeType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description).ToList();
            ThrottleModeList = typeof(ThrottleSetting.ThrottleActivityType).GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description).ToList();
            _selectedPedalAssistLevel = (int)ServoDriveService.PedalSetting.AssistLevel;
            _selectedpedalAssistSensitivities = (int)ServoDriveService.PedalSetting.ActivationTime;
            _selectedthrottleMode = (int)ServoDriveService.ThrottleSetting.ActivityType;
            RaisePropertyChanged(nameof(SelectedPedalAssistLevel));
            RaisePropertyChanged(nameof(SelectedPedalAssistSensitivities));
            RaisePropertyChanged(nameof(SelectedThrottleMode));

            NavigationItems = new Collection<NavigationItem>(new[] {
               new NavigationItem (  "Trip Reports", "Nav/Reports", "" ),
               new NavigationItem (  "Settings", "Nav/Settings", "" ),
               new NavigationItem (  "Configurations", "Nav/Configurations", "" ),
               new NavigationItem (  "Diagnostics", "Nav/Diagnostics", "" ),
               new NavigationItem (  "Logs", "Nav/Logs?createTab=Servo&createTab=Errors&createTab=Events", "" ),
               new NavigationItem (  "Contact Us", "Nav/ContactUs", "" ),
               new NavigationItem (  "About Us", "Nav/AboutUs", "" ),
               new NavigationItem (  "Help", "Nav/Help", "" ),

            });
            
        }

        public List<string> PedalAssistLevelList { get => pedalAssistLevelList; private set => SetProperty(ref pedalAssistLevelList, value); }

        public List<string> PedalAssistSensitivitiesList { get => pedalAssistSensitivitiesList; private set => SetProperty(ref pedalAssistSensitivitiesList, value); }

        public List<string> ThrottleModeList { get => throttleModeList; private set => SetProperty(ref throttleModeList, value); }

        public int SelectedPedalAssistLevel
        {
            get => _selectedPedalAssistLevel; 
            set => SetProperty(ref _selectedPedalAssistLevel, value, () =>
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
            get => _selectedpedalAssistSensitivities; 
            set => SetProperty(ref _selectedpedalAssistSensitivities, value, () =>
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
            get => _selectedthrottleMode; 
            set => SetProperty(ref _selectedthrottleMode, value, () =>
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

        public ServoDriveService ServoDriveService { get; }


    }
    public class NavigationItem
    {
        public NavigationItem(string text, string navigationTarget, string imageIcon)
        {
            NavigationTarget = navigationTarget;
            Text = text;
            ImageIcon = imageIcon;
        }
        public string NavigationTarget { get; set; }

        public string Text { get; set; }

        public string ImageIcon { get; set; }

    }
}
