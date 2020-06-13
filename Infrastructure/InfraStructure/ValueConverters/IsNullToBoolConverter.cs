using System;
using System.Globalization;
using Xamarin.Forms;

namespace Infrastructure.ValueConverters
{
    public class IsNullToBoolConverter : IValueConverter
    {
        public bool Not { get; set; }
        public IsNullToBoolConverter() : this(false)
        {

        }
        public IsNullToBoolConverter(bool not)
        {
            Not = not;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Not)
                return value != null;
            else
                return value == null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool val = (bool)value;
            if (Not)
            {
                if (val != true)
                    return new object();
                else
                    return null;
            }
            else
            {
                if (val != true)
                    return null;
                else
                    return new object();
            }
        }
    }
}
