using Prism.Ioc;
using Prism.Modularity;
using Pedal.Views;
using Pedal.ViewModels;
using Communication.Codec;
using AutoMapper.Configuration;
using DataModels;

namespace Module
{
    public class PedalModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var mapperConfigurationExpression = containerProvider.Resolve<MapperConfigurationExpression>();
            mapperConfigurationExpression.CreateMap<PedalConfigurationPacket, PedalConfiguration>().ReverseMap();
            mapperConfigurationExpression.CreateMap<PedalSettingPacket, PedalSetting>().ReverseMap();
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
