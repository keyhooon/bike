using Prism.Ioc;
using Prism.Modularity;
using SharpCommunication.Base.Codec;
using Communication.Codec;
using SharpCommunication.Base.Channels;
using SharpCommunication.Base.Transport.SerialPort;
using SharpCommunication.Base.Transport;

using Services;

namespace Module
{
    public class CommunicationModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
