using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace bike.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly INavigationService navigationService;
        public MainViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }
        DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand => _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(async (x) =>
        {
            DrawerIsOpen = false;
            await navigationService.NavigateAsync(x);
        }));


        bool _drawerIsOpen;
        public bool DrawerIsOpen { get => _drawerIsOpen; set => SetProperty(ref _drawerIsOpen, value); }

    }
}
