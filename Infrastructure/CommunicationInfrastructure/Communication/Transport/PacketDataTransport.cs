using Acr.UserDialogs.Forms;
using Communication.Communication.Transport;
using Device.Communication.Channels;
using Device.Communication.Codec;
using Infrastructure;
using SharpCommunication.Transport;
using Shiny;
using Shiny.BluetoothLE.Central;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Device.Communication.Transport
{
    public class PacketDataTransport : DataTransport<Packet>
    {
        private static Guid UUID=
        Guid.Parse("00001101-0000-1000-8000-00805F9B34FB");
        private readonly ICentralManager central;
        private readonly IUserDialogs dialogs ;
        private CompositeDisposable ScanDispose;
        public PacketDataTransport( IUserDialogs dialogs, PacketDataTransportOption option) : base(new PacketChannelFactory(), option)
        {
            this.central = ShinyBLE.Central;
            this.dialogs = dialogs;
            ScanDispose = new CompositeDisposable();
        }

        protected override bool IsOpenCore => false;

        protected override void CloseCore()
        {
            ScanDispose.Dispose();
        }

        protected override void OpenCore()
        {
            if (central.Status != AccessState.Available)
                if (!central.TrySetAdapterState(true))
                    dialogs.Alert("Cannot change bluetooth adapter state");
            central.ScanForUniquePeripherals()
                .Subscribe((p) =>
            {
 //               var p = o[0].Peripheral;
                if (!(p is ICanPairPeripherals pair))
                {
                    var txt = "Peripheral not Support Pairing";
                    dialogs.Alert(txt);
                }
                else if (pair.PairingStatus != PairingState.Paired)
                {
                    pair
                        .PairingRequest()
                        .Subscribe(x =>
                        {
                            var txt = x ? "Peripheral Paired Successfully" : "Peripheral Pairing Failed";
                            dialogs.Alert(txt);
                        }).DisposedBy(ScanDispose);
                }
            }).DisposedBy(ScanDispose);
        }
    }
}
