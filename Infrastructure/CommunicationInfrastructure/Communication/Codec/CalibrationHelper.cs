namespace Communication.Codec
{
    public static class CurrentCalibrationHelper
    {
        public readonly static double V_Bias = 1.27d;
        public readonly static double ADC_Max = 4095;
        public readonly static double V_Max = 3.3;
        public readonly static double Coeficient = 0.006;
        public static double Gain => -V_Max / (ADC_Max * Coeficient);
        public static double Bias => V_Bias / Coeficient;
    }
    public static class VoltageCalibrationHelper
    {
        public readonly static double ADC_Max = 4095;
        public readonly static double V_Max = 3.3;
        public readonly static double Coeficient = 60 / 3.3;
        public static double Gain => V_Max * Coeficient / ADC_Max;
        public static double Bias => 0;
    }
}
