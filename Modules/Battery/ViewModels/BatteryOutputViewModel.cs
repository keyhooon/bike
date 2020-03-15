using Battery.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

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
