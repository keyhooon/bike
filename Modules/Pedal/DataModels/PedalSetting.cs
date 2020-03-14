using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pedal.DataModels
{
    public class PedalSetting :BindableBase
    {
        private int _assistLevel;
        public int AssistLevel
        {
            get { return _assistLevel; }
            set { SetProperty(ref _assistLevel, value); }
        }

        private int _activationTime;
        public int ActivationTime
        {
            get { return _activationTime; }
            set { SetProperty(ref _activationTime, value); }
        }
    }
}
