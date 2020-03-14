using Prism.Ioc;
using Prism.Modularity;
using Throttle.Views;
using Throttle.ViewModels;
using Core;
using Communication.Codec;
using SharpCommunication.Base.Codec;

namespace Throttle
{
    public class ThrottleModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA, ViewAViewModel>();
            containerRegistry
                .RegisterInstance(ThrottleConfigurationEncoding.CreateBuilder())
            ;
        }
    }
}
