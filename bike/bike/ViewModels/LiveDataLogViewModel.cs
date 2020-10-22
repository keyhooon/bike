using bike.Services;
using Device;
using Device.Communication.Codec;
using Infrastructure;
using Prism.Mvvm;
using Prism.Navigation;
using SharpCommunication.Channels;
using SharpCommunication.Channels.Decorator;
using SharpCommunication.Codec.Packets;
using SharpCommunication.Transport;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace bike.ViewModels
{
    public class LiveDataLogViewModel : ViewModel
    {
        private readonly ServoDriveService _servoDriveService;
        private readonly DataTransport<Packet> dataTransport;
        IChannel<Packet> channel;
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingViewModel" /> class
        /// </summary>
        public LiveDataLogViewModel(ServoDriveService servoDriveService, DataTransport<Packet> dataTransport)
        {
            _servoDriveService = servoDriveService;
            this.dataTransport = dataTransport;
            _servoDriveService.PropertyChanged += (sender, e)=>RaisePropertyChanged(e.PropertyName);
            channel = dataTransport.Channels.FirstOrDefault();
              if (channel != null)
                channel.DataReceived += ServoViewModel_DataReceived;
            ((INotifyCollectionChanged)dataTransport.Channels).CollectionChanged += ServoViewModel_CollectionChanged;

        }

        private void ServoViewModel_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                channel = (IChannel<Packet>)e.NewItems[0];
                ((IChannel<Packet>)e.NewItems[0]).DataReceived += ServoViewModel_DataReceived;
            }
            if (e.Action == NotifyCollectionChangedAction.Remove)
                channel = null;
                ((IChannel<Packet>)e.NewItems[0]).DataReceived -= ServoViewModel_DataReceived;
        }


        private void ServoViewModel_DataReceived(object sender, DataReceivedEventArg<Packet> e)
        {
            RaisePropertyChanged(nameof(DataReceivedCount));
        }

        #endregion

        public int DataReceivedCount => channel?.ToMonitoredChannel()?.GetDataReceivedCount??0;
        public BatteryOutput BatteryOutput => _servoDriveService.BatteryOutput;

        public CoreSituation Core => _servoDriveService.Core;

        public Fault Fault => _servoDriveService.Fault;

        public ServoInput ServoInput => _servoDriveService.ServoInput;

        public ServoOutput ServoOutput => _servoDriveService.ServoOutput;


    }
}
