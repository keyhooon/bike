using AutoMapper;
using Communication.Codec;
using DataModels;
using Prism.Commands;
using Prism.Mvvm;

using System;


namespace Services
{
    public class ServoManager : BindableBase
    {
        private readonly DataTransportFacade dataTransport;
        private readonly IMapper mapper;

        public ServoManager(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration)
        {
            this.dataTransport = dataTransport;
            ServoInput = new ServoInput();
            ServoOutput = new ServoOutput();
            Fault = new Fault();
            mapper = mapperConfiguration.CreateMapper();
            dataTransport.IsOpenChanged += DataTransport_IsOpenChanged;
            dataTransport.DataReceived += DataTransport_DataReceived;
        }

        private void DataTransport_DataReceived(object sender, PacketReceivedEventArg e)
        {
            switch (e.Packet)
            {
                case ServoInputPacket servoInputPacket:
                    ServoInput = mapper.Map<ServoInputPacket, ServoInput>(servoInputPacket);
                    break;
                case ServoOutputPacket servoOutputPacket:
                    ServoOutput = mapper.Map<ServoOutputPacket, ServoOutput>(servoOutputPacket);
                    break;
                case FaultPacket faultPacket:
                    Fault = mapper.Map<FaultPacket, Fault>(faultPacket);
                    break;
                default:
                    break;
            }
        }

        private void DataTransport_IsOpenChanged(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(IsConnect));
            FaultReceiveCommand.RaiseCanExecuteChanged();
            if (IsConnect)
                dataTransport.CommandTransmit(new ReadCommand() { DataId = FaultPacket.id });
        }

        private DelegateCommand _faultReceiveCommand;
        public DelegateCommand FaultReceiveCommand =>
            _faultReceiveCommand ?? (_faultReceiveCommand = new DelegateCommand(() => {
                dataTransport.CommandTransmit(new ReadCommand() { DataId = FaultPacket.id });
            }, () => dataTransport.IsConnect));


        private DelegateCommand _toggleCruiseCommand;
        public DelegateCommand ToggleCruiseCommand =>
            _toggleCruiseCommand ?? (_toggleCruiseCommand = new DelegateCommand(() => {
                dataTransport.CommandTransmit(new CruiseCommand() { IsOn = !(ServoInput.Cruise > 10) });
            }, () => dataTransport.IsConnect));

        public bool IsConnect => dataTransport.IsConnect;

        private ServoInput _servoInput;
        public ServoInput ServoInput
        {
            get { return _servoInput; }
            protected set { SetProperty(ref _servoInput, value); }
        }

        private ServoOutput _servoOutput;
        public ServoOutput ServoOutput
        {
            get { return _servoOutput; }
            protected set { SetProperty(ref _servoOutput, value); }
        }

        private Fault _fault;
        public Fault Fault
        {
            get { return _fault; }
            protected set { SetProperty(ref _fault, value); }
        }
    }
}
