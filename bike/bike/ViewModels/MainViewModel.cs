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
            servoDriveService.IsOpenChanged += ServoDriveService_IsOpenChanged;
        }

        private void ServoDriveService_IsOpenChanged(object sender, System.EventArgs e)
        {
            IsConnected = servoDriveService.IsOpen;
        }

        DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand => _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(async (x) =>
        {
            await navigationService.NavigateAsync(x);
        }));

        protected override async Task InitAsync(INavigationParameters parameters, CancellationToken? cancellation = null)
        {
            await Task.Run(async() =>
            {
                while(!servoDriveService.IsOpen)
                try
                {
                    if (servoDriveService.CanOpen)
                        servoDriveService.Open();
                        await Task.Delay(1000);
                }
                catch (System.Exception e)
                {

                  //  await dialogs.Alert(e.Message);
                }


            });
        }
        private bool _isConnected;

        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                SetProperty(ref _isConnected, value, () =>
                {
                    if (!IsConnected)
                    {
                        Task.Run(async () =>
                        {
                            while (!IsConnected)
                                try
                                {
                                    await Task.Delay(1000);
                                    if (servoDriveService.CanOpen)
                                        servoDriveService.Open();
                                }
                                catch (System.Exception e)
                                {
                                 //   await dialogs.Alert(e.Message);
                                }
                        });
                    }
                });
            }
        }

    }
}
