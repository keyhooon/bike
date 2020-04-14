using Communication;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Device
{
    public static class ContainerExtension
    {
        public static IContainerRegistry UseServoDrive(this IContainerRegistry containerRegistry)
        {
           return containerRegistry
                .UseCommunication()
                .UseCodec()
                .RegisterSingleton<ServoDriveService>();
        }
    }
}
