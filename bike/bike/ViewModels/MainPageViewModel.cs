using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace bike.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private readonly INavigationService navigationService;
        public MainPageViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        private bool _navigationDrawerIsOpen;
        public bool NavigationDrawerIsOpen
        {
            get { return _navigationDrawerIsOpen; }
            set { SetProperty(ref _navigationDrawerIsOpen, value); }
        }

        private DelegateCommand _navgateDashboard;
        public DelegateCommand NavgateDashboard =>
            _navgateDashboard ?? (_navgateDashboard = new DelegateCommand(async () => 
            await navigationService.NavigateAsync("DashboardPage")));

        private DelegateCommand _navgateSetting;
        public DelegateCommand NavgateSetting =>
            _navgateSetting ?? (_navgateSetting = new DelegateCommand(async () =>
            {
                NavigationDrawerIsOpen = false;
                await navigationService.NavigateAsync("SettingPage");
            }
            ));

        private DelegateCommand _navgateConfiguration;
        public DelegateCommand NavgateConfiguration =>
            _navgateConfiguration ?? (_navgateConfiguration = new DelegateCommand(async () =>
            {
            NavigationDrawerIsOpen = false;
            await navigationService.NavigateAsync("ConfigurationPage");
            }));

        private DelegateCommand _navgateHelp;
        public DelegateCommand NavgateHelp =>
            _navgateHelp ?? (_navgateHelp = new DelegateCommand(async () =>
            {
            NavigationDrawerIsOpen = false;
            await navigationService.NavigateAsync("HelpPage");
            }));

        private DelegateCommand _navgateFeedback;
        public DelegateCommand NavgateFeedback =>
            _navgateFeedback ?? (_navgateFeedback = new DelegateCommand(async () =>
            {
            NavigationDrawerIsOpen = false;
            await navigationService.NavigateAsync("FeedbackPage");
            }));

        private DelegateCommand _navgateReview;
        public DelegateCommand NavgateReview =>
            _navgateReview ?? (_navgateReview = new DelegateCommand(async () =>
            {
            NavigationDrawerIsOpen = false;
            await navigationService.NavigateAsync("ContactUsPage");
            }));

        private DelegateCommand _navgateAboutUs;
        public DelegateCommand NavgateAboutUs =>
            _navgateAboutUs ?? (_navgateAboutUs = new DelegateCommand(async () =>
            {
            NavigationDrawerIsOpen = false;
            await navigationService.NavigateAsync("AboutUsSimplePage");
            }));


    }
}
