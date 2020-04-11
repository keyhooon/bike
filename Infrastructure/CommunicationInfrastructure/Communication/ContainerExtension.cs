using Communication.Codec;
using Prism.Ioc;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Codec;
using SharpCommunication.Base.Transport;
using SharpCommunication.Base.Transport.SerialPort;

namespace Communication
{
    public static class ContainerExtension
    {
        public static IContainerRegistry UseCommunication(this IContainerRegistry containerRegistry)
        {
            return containerRegistry.Register<ICodec<DevicePacket>, DevicePacketCodec>()
            .Register<IChannelFactory<DevicePacket>, ChannelFactory<DevicePacket>>()
            .Register<DataTransport<DevicePacket>, SerialPortDataTransport<DevicePacket>>()
            .RegisterInstance(new SerialPortDataTransportOption("com6", 115200))
            .RegisterSingleton<DataTransportFacade>();
        }

    }
}
