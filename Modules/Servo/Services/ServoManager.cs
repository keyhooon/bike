using AutoMapper;
using Communication.Codec;
using DataModels;
using Prism.Commands;
using SharpCommunication.Base.Codec.Packets;


namespace Services
{
    public class ServoManager : HardwareService
    {
        public ServoManager(DataTransportFacade dataTransport, IConfigurationProvider mapperConfiguration) :base(dataTransport, mapperConfiguration)
        {

            ServoInput = new ServoInput();
            ServoOutput = new ServoOutput();
            Fault = new Fault();
        }

        protected override void DataReceivedHandle(IAncestorPacket packet)
        {
            switch (packet)
            {
                case ServoInputPacket servoInputPacket:
                    mapper.Map(servoInputPacket, ServoInput);
                    break;
                case ServoOutputPacket servoOutputPacket:
                    mapper.Map(servoOutputPacket, ServoOutput);
                    break;
                case FaultPacket faultPacket:
                    mapper.Map(faultPacket, Fault);
                    break;
                default:
                    break;
            }
        }

        protected override void IsConnectedChangedHandle()
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
            }, () => IsConnect).ObservesProperty(() => nameof(IsConnect)));


        private DelegateCommand _toggleCruiseCommand;
        public DelegateCommand ToggleCruiseCommand =>
            _toggleCruiseCommand ?? (_toggleCruiseCommand = new DelegateCommand(() => {
                dataTransport.CommandTransmit(new CruiseCommand() { IsOn = !(ServoInput.Cruise > 10) });
            }, () => IsConnect).ObservesProperty(() => nameof(IsConnect)));


        public ServoInput ServoInput
        {
            get;
        }

        public ServoOutput ServoOutput
        {
            get;
        }

        public Fault Fault
        {
            get;
        }
    }
}
