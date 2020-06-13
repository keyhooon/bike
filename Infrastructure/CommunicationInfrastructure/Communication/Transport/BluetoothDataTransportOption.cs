
using SharpCommunication.Transport;

namespace Device.Communication.Transport
{
    public class BluetoothDataTransportOption : DataTransportOption
    {

        public BluetoothDataTransportOption(string uUID, string deviceName)
        {
            UUID = uUID;
            DeviceName = deviceName;
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
                OnPropertyChanged();
            }
        }
    }
}