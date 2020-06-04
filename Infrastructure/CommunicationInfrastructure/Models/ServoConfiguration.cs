using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;

namespace DataModels
{
    public class ServoConfiguration : BindableBase
    {

        private double _wheelRadius;
        public double WheelRadius
        {
            get => _wheelRadius;
            set => SetProperty(ref _wheelRadius, value);
        }
    }
}
