using DataModels;
using Prism.Mvvm;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel;

namespace bike.ViewModels.Main
{
    public class DashboardViewModel : BindableBase
    {
        private readonly ServoManager servomanager;
        private readonly BatteryManager batteryManager;
        private readonly LightManager lightManager;
        private readonly ThrottleManager throttleManager;
        private readonly PedalManager pedalManager;

        public DashboardViewModel(ServoManager servomanager, BatteryManager batteryManager, LightManager lightManager, ThrottleManager throttleManager, PedalManager pedalManager)
        {
            this.servomanager = servomanager;
            this.batteryManager = batteryManager;
            this.lightManager = lightManager;
            this.throttleManager = throttleManager;
            this.pedalManager = pedalManager;

            throttleManager.ThrottleSetting.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(throttleManager.ThrottleSetting.ThrottleActivity))
                    RaisePropertyChanged(nameof(ThrottleActivity));
            };

            pedalManager.PedalSetting.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(pedalManager.PedalSetting.ActivationTime))
                    RaisePropertyChanged(nameof(ActivationTime));
                else if (e.PropertyName == nameof(pedalManager.PedalSetting.AssistLevel))
                    RaisePropertyChanged(nameof(AssistLevel));
            };

            servomanager.ServoOutput.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(servomanager.ServoOutput.WheelSpeed))
                    RaisePropertyChanged(nameof(WheelSpeed));
                else if (e.PropertyName == nameof(servomanager.ServoOutput.Activity))
                    RaisePropertyChanged(nameof(Activity));
            };
            servomanager.ServoInput.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(servomanager.ServoInput.Break))
                    RaisePropertyChanged(nameof(IsBreak));
                else if (e.PropertyName == nameof(servomanager.ServoInput.Fault))
                    RaisePropertyChanged(nameof(IsFault));
                else if (e.PropertyName == nameof(servomanager.ServoInput.Cruise))
                    RaisePropertyChanged(nameof(Cruise));
            };

            batteryManager.BatteryConfiguration.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(batteryManager.BatteryConfiguration.OverVoltage))
                    RaisePropertyChanged(nameof(OverVoltage));
                else if (e.PropertyName == nameof(batteryManager.BatteryConfiguration.UnderVoltage))
                    RaisePropertyChanged(nameof(UnderVoltage));
                else if (e.PropertyName == nameof(batteryManager.BatteryConfiguration.OverCurrent))
                    RaisePropertyChanged(nameof(OverCurrent));
                else if (e.PropertyName == nameof(batteryManager.BatteryConfiguration.OverTemprature))
                    RaisePropertyChanged(nameof(OverTemprature));
            };
            batteryManager.BatteryOutput.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(batteryManager.BatteryOutput.Current))
                    RaisePropertyChanged(nameof(Current));
                else if (e.PropertyName == nameof(batteryManager.BatteryOutput.Voltage))
                    RaisePropertyChanged(nameof(Voltage));
                else if (e.PropertyName == nameof(batteryManager.BatteryOutput.Temprature))
                    RaisePropertyChanged(nameof(Temprature));

            };
        
            lightManager.LightState.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(lightManager.LightState.Lights))
                    RaisePropertyChanged(nameof(Lights));
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

        public ThrottleActivityType ThrottleActivity
        {
            get
            {
                return throttleManager.ThrottleSetting.ThrottleActivity;
            }
            set
            {
                if (throttleManager.ThrottleSetting.ThrottleActivity == value)
                    return;
                throttleManager.ThrottleSetting.ThrottleActivity = value;
            }
        }
        public ActivationTimeType ActivationTime
        {
            get
            {
                return pedalManager.PedalSetting.ActivationTime;
            }
            set
            {
                if (pedalManager.PedalSetting.ActivationTime == value)
                    return;
                pedalManager.PedalSetting.ActivationTime = value;
            }
        }
        public AssistLevelType AssistLevel
        {
            get
            {
                return pedalManager.PedalSetting.AssistLevel;
            }
            set
            {
                if (pedalManager.PedalSetting.AssistLevel == value)
                    return;
                pedalManager.PedalSetting.AssistLevel = value;
            }
        }




    }
}
