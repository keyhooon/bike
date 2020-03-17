using Prism.Ioc;
using Prism.Modularity;
using Communication.Codec;
using AutoMapper.Configuration;
using DataModels;
using Core.Views;
using Core.ViewModels;

namespace Module
{
    public class CoreModule: IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var mapperConfigurationExpression = containerProvider.Resolve<MapperConfigurationExpression>();
            mapperConfigurationExpression.CreateMap<CoreConfigurationPacket, CoreVersion>().ReverseMap();
            mapperConfigurationExpression.CreateMap<CoreSituationPacket, CoreSituation>().ReverseMap();
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
