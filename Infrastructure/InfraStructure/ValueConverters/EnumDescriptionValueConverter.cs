using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace Core.ValueConverters
{
    public class EnumDescriptionValueConverter : IValueConverter
    {

        //public Type EnumType { get; set; }



        //public EnumDescriptionValueConverter()
        //{

        //}
        //public EnumDescriptionValueConverter(Type enumType)
        //{
        //    EnumType = enumType;
        //}
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi != null)
            {
                var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return attributes.Length > 0 && !string.IsNullOrEmpty(attributes[0].Description) ? attributes[0].Description : value.ToString();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        //public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        //{
        //    FieldInfo[] fis = EnumType.GetFields();
        //    foreach (var fi in fis)
        //    {
        //        if (fi != null)
        //        {
        //            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        //            if (attributes.Length > 0 && (!string.IsNullOrEmpty(attributes[0].Description) ? attributes[0].Description : value.ToString()) == value)
        //            { 
        //                return System.Convert.ChangeType(fi.GetRawConstantValue(), EnumType);
        //            }
        //        }
        //    }

        //    return string.Empty;
        //}
    }
}
