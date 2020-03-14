

using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Battery.DataModel
{
    public class BatteryConfiguration : BindableBase
    {
        private double _overCurrent;
        public double OverCurrent
        {
            get { return _overCurrent; }
            set { SetProperty(ref _overCurrent, value); }
        }

        private double _overVoltage;
        public double OverVoltage
        {
            get { return _overVoltage; }
            set { SetProperty(ref _overVoltage, value); }
        }

        private double _underVotlage;
        public double UnderVoltage
        {
            get { return _underVotlage; }
            set { SetProperty(ref _underVotlage, value); }
        }

        private double _nominalVoltage;
        public double NominalVoltage
        {
            get { return _nominalVoltage; }
            set { SetProperty(ref _nominalVoltage, value); }
        }

        private double _overTemprature;
        public double OverTemprature
        {
            get { return _overTemprature; }
            set { SetProperty(ref _overTemprature, value); }
        }

    }
}
