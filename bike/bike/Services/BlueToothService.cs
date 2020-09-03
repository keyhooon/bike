using System;
using System.Collections.Generic;
using System.Text;

namespace bike.Services
{
    public interface IBlueToothService
    {
        public (string Name, string Address)[] BluetoothBonded { get; }
    }
}
