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
    public class MainViewModel : ViewModel
    {
        private readonly INavigationService navigationService;
        private readonly ServoDriveService servoDriveService;
        private readonly IUserDialogs dialogs;

        public MainViewModel(INavigationService navigationService, ServoDriveService servoDriveService, IUserDialogs dialogs)
        {
            this.navigationService = navigationService;
            this.servoDriveService = servoDriveService;
            this.dialogs = dialogs;
            servoDriveService.IsOpenChanged += (sender, e)=>  RaisePropertyChanged(nameof(IsConnected));
        }

        DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand => _navigateCommand ??= new DelegateCommand<string>( (x) =>
        {
            navigationService.NavigateAsync(x);
        });

        public bool IsConnected
        {
            get => servoDriveService.IsOpen;
        }

    }
}
