using Prism.Ioc;
using Prism.Modularity;
using CoreModule.Views;
using CoreModule.ViewModels;
using Core;
using Communication.Codec;
using SharpCommunication.Base.Codec;

namespace CoreModule
{
    public class CoreModuleModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA, ViewAViewModel>();
            containerRegistry
                .RegisterInstance(CoreSituationEncoding.CreateBuilder())
                .RegisterInstance(CoreConfigurationEncoding.CreateBuilder())
                ;
        }
    }
}
