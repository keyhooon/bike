using Device.Communication.Codec;
using Device.Communication.Transport;
using SharpCommunication.Channels;
using SharpCommunication.Codec.Packets;
using SharpCommunication.Transport;
using SharpCommunication.Transport.SerialPort;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Device.Communication.Service
{
    class DeviceService
    {
        private List<DataTransport<Packet>> DataTransports;
        public DeviceService()
        {
            DataTransports = new List<DataTransport<Packet>>();
        }

        public void Start()
        {
            foreach (var dataTransport in DataTransports)
            {
                dataTransport.Open();
            }
        }
        public void Stop()
        {
            foreach (var dataTransport in DataTransports)
            {
                dataTransport.Close();
            }
        }
        public void RegisterDataTransport(DataTransport<Packet> dataTransport)
        {
            DataTransports.Add(dataTransport);
        }
        
    }
}
