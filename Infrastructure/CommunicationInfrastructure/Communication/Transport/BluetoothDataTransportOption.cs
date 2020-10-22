
using SharpCommunication.Transport;
using Shiny.Settings;

namespace Device.Communication.Transport
{
    public class BluetoothDataTransportOption : DataTransportOption
    {
        private readonly ISettings settings;

        public BluetoothDataTransportOption(ISettings settings)
        {

            this.settings = settings;
            _uUID = settings.Get(nameof(UUID), "00001101-0000-1000-8000-00805F9B34FB");
            _deviceName = settings.Get(nameof(DeviceName), "ICRC-UGI");
        }
        private string _uUID;
        public string UUID
        {
            get => _uUID;
            set
            {
                if (_uUID == value)
                    return;
                _uUID = value;
                settings.Set(nameof(UUID), value);
                OnPropertyChanged();
            }
        }
        private string _deviceName;


        public string DeviceName
        {
            get => _deviceName;
            set
            {
                if (_deviceName == value)
                    return;
                _deviceName = value;
                settings.Set(nameof(DeviceName), value);
                OnPropertyChanged();
            }
        }

    }
}