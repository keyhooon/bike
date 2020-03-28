using Prism.Mvvm;

namespace DataModels
{
    public class ServoOutput :BindableBase
    {
        private double _activity;
        public double Activity
        {
            get { return _activity; }
            set { SetProperty(ref _activity, value); }
        }

        private double _wheelSpeed;
        public double WheelSpeed
        {
            get { return _wheelSpeed; }
            set { SetProperty(ref _wheelSpeed, value); }
        }

    }
}
