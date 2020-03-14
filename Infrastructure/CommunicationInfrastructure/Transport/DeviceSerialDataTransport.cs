
using Communication.Codec;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Transport.SerialPort;
using static Communication.Channels.DevicePacketChannel;

namespace Communication.Transport
{
    public class DeviceSerialDataTransport : SerialPortDataTransport<DevicePacket>
    {
        public DeviceSerialDataTransport(SerialPortDataTransportOption option) : base(new DevicePacketChannelFactory(), option)
        {

        }
    }
}
