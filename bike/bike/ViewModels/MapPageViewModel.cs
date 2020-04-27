using Acr.UserDialogs.Forms;
using Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Shiny.Locations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bike.ViewModels
{
    public class MapPageViewModel : ViewModel
    {
        private readonly IGpsManager manager;
        private readonly IUserDialogs dialogs;

        public MapPageViewModel(IGpsManager manager, IUserDialogs dialogs)
        {
            this.manager = manager;
            this.dialogs = dialogs;
        }
        public async override void OnAppearing()
        {
            base.OnAppearing();
            var result = await dialogs.RequestAccess(() => this.manager.RequestAccess(new GpsRequest { UseBackground = true}));
            if (!result)
            {
                await dialogs.Alert("Insufficient permissions");
                return;
            }

            var request = new GpsRequest
            {
                UseBackground = true,
                Priority = GpsPriority.Normal,
            };

            await manager.StartListener(request);
        }
        public async override void OnDisappearing()
        {
            base.OnDisappearing();
            await manager.StopListener();

        }
    }
}
