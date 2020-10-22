using Device.Communication.Transport;
using Prism.Ioc;
using bike.Services;

namespace bike.Services
{
    public static class ContainerExtension
    {
        public static IContainerRegistry UseServoDrive(this IContainerRegistry containerRegistry)
        {
            return containerRegistry
                .RegisterSingleton<BluetoothDataTransportOption>()
                .RegisterSingleton<ServoDriveService>(); 
        }
    }
}
