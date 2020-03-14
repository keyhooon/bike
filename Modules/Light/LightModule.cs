using Prism.Ioc;
using Prism.Modularity;
using Light.Views;
using Light.ViewModels;
using Core;
using Light.DataTrandferPackets;
using Communication.Codec;
using SharpCommunication.Base.Codec;

namespace Light
{
    public class LightModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA, ViewAViewModel>();
            containerRegistry
                .RegisterInstance(LightSettingPacketEncoding.CreateBuilder())
                .RegisterInstance(LightStatePacetEncoding.CreateBuilder())
                .RegisterInstance(LightCommandEncoding.CreateBuilder())
                ;
        }
    }
}
