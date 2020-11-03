using Acr.UserDialogs.Forms;
using bike.Services;
using Device.Communication.Codec;
using Device.Communication.Transport;
using Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using SharpCommunication.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace bike.ViewModels
{
    public class BluetoothPageViewModel : AbstractItemListViewModel<Tuple<string ,string >>
    {
        private readonly IUserDialogs dialogs;
        private readonly DataTransport<Packet> dataTransport;
        private readonly BluetoothDataTransportOption option;
        private readonly IBlueToothService blueToothService;
        private (string Name, string Address)[] bluetoothBonded;
        public BluetoothPageViewModel(IUserDialogs dialogs, DataTransport<Packet> dataTransport, BluetoothDataTransportOption option, IBlueToothService blueToothService) : base(dialogs)
        {
            this.dialogs = dialogs;
            this.dataTransport = dataTransport;
            this.option = option;
            this.blueToothService = blueToothService;
            CanClear = false;
            CanShowDetail = false;
        }

        private DelegateCommand<Tuple<string, string>> _selectBluetoothCommand;
        public DelegateCommand<Tuple<string, string>> SelectBluetoothCommand =>
            _selectBluetoothCommand ??= new DelegateCommand<Tuple<string, string>>(async (b) =>
            {
                var confirm = await dialogs.Confirm($"Do you want Connect {b.Item1}");
                if (!confirm) return;
                option.DeviceName = b.Item1;
                //option.UUID = b.Item2;
                if (dataTransport.IsOpen)
                    dataTransport.Close();

            });

        protected override async Task<IEnumerable<Tuple<string, string>>> LoadItemsAsync(INavigationParameters parameters, CancellationToken token)
        {
            await Task.CompletedTask;
            var result = blueToothService.BluetoothBonded;
            return result.Select((o) => new Tuple<string, string>(o.Name, o.Address));
        }


        protected override Task ClearItemsAsync(CancellationToken token)
        {
            throw new NotImplementedException();
        }

        protected override string DatailText(Tuple<string, string> item)
        {
            throw new NotImplementedException();
        }

        protected override string DetailHeader(Tuple<string, string> item)
        {
            throw new NotImplementedException();
        }
    }
}
