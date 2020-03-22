using Prism.Mvvm;

namespace DataModels
{
    public class ServoInput :BindableBase
    {

        private double _throttle;
        public double Throttle
        {
            get { return _throttle; }
            set { SetProperty(ref _throttle, value); }
        }

        private double _pedal;
        public double Pedal
        {
            get { return _pedal; }
            set { SetProperty(ref _pedal, value); }
        }

        private double _cruise;
        public double Cruise
        {
            get { return _cruise; }
            set { SetProperty(ref _cruise, value); }
        }

        private bool _break;
        public bool Break
        {
            get { return _break; }
            set { SetProperty(ref _break, value); }
        }

        private bool _fault;
        public bool Fault
        {
            get { return _fault; }
            set { SetProperty(ref _fault, value); }
        }


    }
}
