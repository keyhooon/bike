using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace bike.Controls.SKColorDispersionBar
{
    public struct DispersionSpan : IDispersionSpan
    {
        public DispersionSpan(Color color, double value)
        {
            Color = color;
            Value = value;
        }

        public Color Color { get; }

        public double Value { get; private set; }

        public void IncrementValue(double value)
        {
            Value += value;
        }
    }
}
