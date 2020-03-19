using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataModels
{
    public class PedalSetting :BindableBase
    {
        private AssistLevelType _assistLevel;
        public AssistLevelType AssistLevel
        {
            get { return _assistLevel; }
            set { SetProperty(ref _assistLevel, value); }
        }

        private ActivationTimeType _activationTime;
        public ActivationTimeType ActivationTime
        {
            get { return _activationTime; }
            set { SetProperty(ref _activationTime, value); }
        }

        public enum AssistLevelType
        {
            EightySevenPointFive,
            SeventyFive,
            SixtyTwoPointFive,
            Fifty,
            ThirtySevenPointFive,
            ThrityOnePointTwentyFive,
            TowentyFive,
            Off,
        }

        public enum ActivationTimeType
        {
            ExtraSensitive,
            VerySensitive,
            Sensitive,
            LowSensitive,
        }
    }
}
