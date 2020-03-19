using DataModels;
using Prism.Commands;
using Prism.Mvvm;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace bike.ViewModels.Main
{
    public class DashboardViewModel : BindableBase
    {
        private readonly ServoManager servomanager;
        private readonly BatteryManager batteryManager;

        private readonly LightManager lightManager;

        public DashboardViewModel(ServoManager servomanager, BatteryManager batteryManager, LightManager lightManager)
        {
            this.servomanager = servomanager;
            this.batteryManager = batteryManager;

            this.lightManager = lightManager;

            this.servomanager.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(servomanager.ServoOutput))
                {
                    RaisePropertyChanged(nameof(WheelSpeed));
                    RaisePropertyChanged(nameof(Activity));
                }
                if (e.PropertyName == nameof(servomanager.ServoInput))
                {
                    RaisePropertyChanged(nameof(IsBreak));
                    RaisePropertyChanged(nameof(IsFault));
                    RaisePropertyChanged(nameof(Cruise));
                }
            };
            this.batteryManager.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(batteryManager.BatteryConfiguration))
                {
                    RaisePropertyChanged(nameof(OverVoltage));
                    RaisePropertyChanged(nameof(UnderVoltage));
                    RaisePropertyChanged(nameof(OverCurrent));
                    RaisePropertyChanged(nameof(OverTemprature));
                }
                if (e.PropertyName == nameof(batteryManager.BatteryOutput))
                {
                    RaisePropertyChanged(nameof(Current));
                    RaisePropertyChanged(nameof(Voltage));
                    RaisePropertyChanged(nameof(Temprature));
                }
            };
            this.lightManager.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(lightManager.LightState))
                {
                    RaisePropertyChanged(nameof(Lights));
                }
            };


        }

        public double WheelSpeed => servomanager.ServoOutput.WheelSpeed;
        public double Activity => servomanager.ServoOutput.Activity;
        public bool IsBreak => servomanager.ServoInput.Break;
        public bool IsFault => servomanager.ServoInput.Fault;
        public double Cruise => servomanager.ServoInput.Cruise;

        public double OverCurrent => batteryManager.BatteryConfiguration.OverCurrent;
        public double Current => batteryManager.BatteryOutput.Current;

        public double OverVoltage => batteryManager.BatteryConfiguration.OverVoltage;
        public double UnderVoltage => batteryManager.BatteryConfiguration.UnderVoltage;
        public double Voltage => batteryManager.BatteryOutput.Voltage;

        public double OverTemprature => batteryManager.BatteryConfiguration.OverTemprature;
        public double Temprature => batteryManager.BatteryOutput.Temprature;

        public bool[] Lights => lightManager.LightState.Lights.ToArray();



    }
}
