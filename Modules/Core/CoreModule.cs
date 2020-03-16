using Prism.Ioc;
using Prism.Modularity;
using CoreModule.Views;
using CoreModule.ViewModels;
using Core;
using Communication.Codec;
using SharpCommunication.Base.Codec;
using AutoMapper.Configuration;
using Core.DataModels;

namespace CoreModule
{
    public class CoreModuleModule : IModule
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
