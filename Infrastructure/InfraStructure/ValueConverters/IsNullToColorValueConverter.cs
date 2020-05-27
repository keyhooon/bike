using System;
using System.Globalization;
using Xamarin.Forms;

namespace Infrastructure.ValueConverters
{
    public class IsNullToColorValueConverter : IValueConverter
    {
        public Color IsNullColor { get; set; }
        public Color NotNullColor { get; set; }
        public IsNullToColorValueConverter() : this (Color.Red,Color.Green)
        {

        }
        public IsNullToColorValueConverter(Color isNullColor, Color notNullColor)
        {
            IsNullColor = isNullColor;
            NotNullColor = notNullColor;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return IsNullColor;
            else
                return NotNullColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Color c))
                return null;
            else if (c == IsNullColor)
                return IsNullColor;
            else
                return NotNullColor;
            
        }
    }
}
