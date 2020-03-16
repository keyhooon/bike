using Prism.Ioc;
using Prism.Modularity;
using Light.Views;
using Light.ViewModels;
using Core;
using Light.DataTrandferPackets;
using Communication.Codec;
using SharpCommunication.Base.Codec;
using Light.DataModels;
using AutoMapper.Configuration;

namespace Light
{
    public class LightModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var mapperConfigurationExpression = containerProvider.Resolve<MapperConfigurationExpression>();
            mapperConfigurationExpression.CreateMap<LightSettingPacket, LightSetting>().ReverseMap();
            mapperConfigurationExpression.CreateMap<LightStatePacket, LightState>().ReverseMap();
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
