using Prism.Ioc;
using Prism.Modularity;
using Pedal.Views;
using Pedal.ViewModels;
using Core;
using Pedal.DataTrandferPackets;
using Communication.Codec;
using SharpCommunication.Base.Codec;

namespace Pedal
{
    public class PedalModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ViewA, ViewAViewModel>();
            containerRegistry
                .RegisterInstance(PedalConfigurationEncoding.CreateBuilder())
                .RegisterInstance(PedalSettingEncoding.CreateBuilder())
                ;
        }
    }
}
