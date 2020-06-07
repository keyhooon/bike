using Device;
using Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Threading;
using System.Threading.Tasks;

namespace bike.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private readonly INavigationService navigationService;
        private readonly ServoDriveService servoDriveService;

        public MainViewModel(INavigationService navigationService, ServoDriveService servoDriveService)
        {
            this.navigationService = navigationService;
            this.servoDriveService = servoDriveService;
        }
        DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand => _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(async (x) =>
        {
            await navigationService.NavigateAsync(x);
        }));

        protected override async Task LoadAsync(INavigationParameters parameters, CancellationToken? cancellation = null)
        {
            servoDriveService.Open();
        }

    }
}
