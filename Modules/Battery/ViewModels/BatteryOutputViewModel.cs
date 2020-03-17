using Prism.Mvvm;
using Services;

namespace Battery.ViewModels
{
    public class BatteryOutputViewModel : BindableBase
    {
        public BatteryManager BatteryManager { get; }

        public BatteryOutputViewModel(BatteryManager batteryManager)
        {
            BatteryManager = batteryManager;
        }
    }
}
