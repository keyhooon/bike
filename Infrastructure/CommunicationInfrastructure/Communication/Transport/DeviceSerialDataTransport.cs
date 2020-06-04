using Device.Communication.Channels;
using Device.Communication.Codec;
using SharpCommunication.Transport.SerialPort;

namespace Device.Communication.Transport
{
    public class DeviceSerialDataTransport : SerialPortDataTransport<Packet>
    {
        public DeviceSerialDataTransport(SerialPortDataTransportOption option) : base(new DevicePacketChannelFactory(), option)
        {

        }
    }
}
