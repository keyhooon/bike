using Communication.Codec;
using Prism.Ioc;
using SharpCommunication.Channels;
using SharpCommunication.Codec;
using SharpCommunication.Transport;
using SharpCommunication.Transport.SerialPort;
using SharpCommunication.Codec;
using Device.Communication.Codec;

namespace Communication
{
    public static class ContainerExtension
    {
        public static IContainerRegistry UseCommunication(this IContainerRegistry containerRegistry)
        {
            return containerRegistry.Register<ICodec<Packet>, DevicePacketCodec>()


            .Register<IChannelFactory<Packet>, ChannelFactory<Packet>>()
            .Register<DataTransport<Packet>, SerialPortDataTransport<Packet>>()
            .RegisterInstance(new SerialPortDataTransportOption("com6", 115200))
            .RegisterSingleton<DataTransportFacade>();
        }
        public static IContainerRegistry UseCodec(this IContainerRegistry containerRegistry)
        {
            return containerRegistry
                 .RegisterInstance(BatteryConfiguration.Encoding.CreateBuilder())
                 .RegisterInstance(BatteryOutput.Encoding.CreateBuilder())
                 .RegisterInstance(CoreConfiguration.Encoding.CreateBuilder())
                 .RegisterInstance(CoreSituation.Encoding.CreateBuilder())
                 .RegisterInstance(CruiseCommand.Encoding.CreateBuilder())
                 .RegisterInstance(Fault.Encoding.CreateBuilder())
                 .RegisterInstance(LightCommand.Encoding.CreateBuilder())
                 .RegisterInstance(LightSetting.Encoding.CreateBuilder())
                 .RegisterInstance(LightState.Encoding.CreateBuilder())
                 .RegisterInstance(PedalConfiguration.Encoding.CreateBuilder())
                 .RegisterInstance(PedalSetting.Encoding.CreateBuilder())
                 .RegisterInstance(ServoInput.Encoding.CreateBuilder())
                 .RegisterInstance(ServoOutput.Encoding.CreateBuilder())
                 .RegisterInstance(ThrottleConfiguration.Encoding.CreateBuilder())
                 ;
        }
    }
}
