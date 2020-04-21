using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace bike.Controls
{
    public interface IDispersionSpan
    {
        Color Color { get; }

        double Value { get; }

        void IncrementValue(double value);
    }
}
