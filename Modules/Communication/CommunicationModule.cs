using Prism.Ioc;
using Prism.Modularity;
using Communication.Views;
using Communication.ViewModels;
using SharpCommunication.Base.Codec;
using Communication.Codec;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Transport.SerialPort;
using SharpCommunication.Base.Transport;

namespace Communication
{
    public class CommunicationModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA, ViewAViewModel>();
            containerRegistry.Register< ICodec<DevicePacket>, DevicePacketCodec >();
            containerRegistry.RegisterSingleton< IChannelFactory<DevicePacket>, ChannelFactory<DevicePacket> >();
            containerRegistry.RegisterSingleton< DataTransport<DevicePacket>, SerialPortDataTransport<DevicePacket> >();
        }
    }
}
