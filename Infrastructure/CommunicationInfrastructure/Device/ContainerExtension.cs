using Communication;
using Communication.Communication.Transport;
using Device.Communication.Codec;
using Device.Communication.Transport;
using Prism.Ioc;
using SharpCommunication.Transport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Device
{
    public static class ContainerExtension
    {
        public static IContainerRegistry UseServoDrive(this IContainerRegistry containerRegistry)
        {
            return containerRegistry
                .Register<DataTransport<Packet>, PacketDataTransport>()
                .RegisterInstance(new PacketDataTransportOption())
                .RegisterSingleton<ServoDriveService>(); 
        }
    }
}
