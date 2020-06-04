
using Infrastructure.TypeConverters;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Prism.Mvvm;
using System.ComponentModel;

namespace DataModels
{

    public class LightSetting : BindableBase
    {

        private LightVolume _light1;

        public LightVolume Light1
        {
            get => _light1;
            set => SetProperty(ref _light1, value);
        }


        private LightVolume _light2;

        public LightVolume Light2
        {
            get => _light2;
            set => SetProperty(ref _light2, value);
        }


        private LightVolume _light3;

        public LightVolume Light3
        {
            get => _light3;
            set => SetProperty(ref _light3, value);
        }


        private LightVolume _light4;

        public LightVolume Light4
        {
            get => _light4;
            set => SetProperty(ref _light4, value);
        }
    }
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum LightVolume : byte
    {
        High = 3,
        Normal = 2,
        Low = 1,
        ExtraLow = 0
    }
}
