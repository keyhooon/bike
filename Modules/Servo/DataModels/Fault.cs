using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class Fault :BindableBase
    {
        private bool _overCurrent;
        public bool OverCurrent
        {
            get { return _overCurrent; }
            set { SetProperty(ref _overCurrent, value); }
        }

        private bool _overTemprature;
        public bool OverTemprature
        {
            get { return _overTemprature; }
            set { SetProperty(ref _overTemprature, value); }
        }

        private bool _pedalSensor;
        public bool PedalSensor
        {
            get { return _pedalSensor; }
            set { SetProperty(ref _pedalSensor, value); }
        }

        private bool _throttle;
        public bool Throttle
        {
            get { return _throttle; }
            set { SetProperty(ref _throttle, value); }
        }

        private bool _overVoltage;
        public bool OverVoltage
        {
            get { return _overVoltage; }
            set { SetProperty(ref _overVoltage, value); }
        }

        private bool _underVoltage;
        public bool UnderVoltage
        {
            get { return _underVoltage; }
            set { SetProperty(ref _underVoltage, value); }
        }

        private bool _motor;
        public bool Motor
        {
            get { return _motor; }
            set { SetProperty(ref _motor, value); }
        }

        private bool _driver;
        public bool Driver
        {
            get { return _driver; }
            set { SetProperty(ref _driver, value); }
        }

    }
}
