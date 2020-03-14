using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pedal.DataModels
{
    public class PedalConfiguration : BindableBase
    {
        private int _magnetCount;
        public int MagnetCount
        {
            get { return _magnetCount; }
            set { SetProperty(ref _magnetCount, value); }
        }

    }
}
