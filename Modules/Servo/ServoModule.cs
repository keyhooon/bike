using Prism.Ioc;
using Prism.Modularity;
using Servo.Views;
using Servo.ViewModels;
using Core;
using Servo.DataTrandferPackets;
using Communication.Codec;
using SharpCommunication.Base.Codec;

namespace Servo
{
    public class ServoModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA, ViewAViewModel>();
            containerRegistry
                .RegisterInstance(FaultEncoding.CreateBuilder())
                .RegisterInstance(ServoInputEncoding.CreateBuilder())
                .RegisterInstance(ServoOutputEncoding.CreateBuilder())
            ;
        }
    }
}
