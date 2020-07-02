using Device;
using Infrastructure;
using Prism.Commands;
using bike.Services;

using Prism.Navigation;
using System.Threading;
using System.Threading.Tasks;
using Acr.UserDialogs.Forms;

namespace bike.ViewModels
{
    public class MainViewModel : ViewModel, IMasterDetailPageOptions
    {
        private readonly INavigationService navigationService;
        private readonly ServoDriveService servoDriveService;
        private readonly IUserDialogs dialogs;

        public MainViewModel(INavigationService navigationService, ServoDriveService servoDriveService, IUserDialogs dialogs)
        {
            this.navigationService = navigationService;
            this.servoDriveService = servoDriveService;
            this.dialogs = dialogs;
            IsPresentedAfterNavigation = false;
            servoDriveService.IsOpenChanged += (sender, e) => RaisePropertyChanged(nameof(IsConnected));
        }

        DelegateCommand<string> _navigateCommand;
        private bool _masterIsPresent;

        public DelegateCommand<string> NavigateCommand => _navigateCommand ??= new DelegateCommand<string>(async (x) =>
        {
            IsPresented = false;
            await Task.Delay(300);
            await navigationService.NavigateAsync(x);
            
        });

        public bool IsConnected
        {
            get => servoDriveService.IsOpen;
        }

        public bool IsPresented
        {
            get => _masterIsPresent; set => SetProperty(ref _masterIsPresent, value);
        }

        public bool IsPresentedAfterNavigation { get; set; }
    }
}
