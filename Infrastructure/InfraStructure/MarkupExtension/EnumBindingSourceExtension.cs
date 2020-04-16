using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Infrastructure.MarkupExtension
{
    [ContentProperty("EnumType")]
    public class EnumBindingSourceExtension : IMarkupExtension
    {
        private Type _enumType;
        public Type EnumType
        {
            get { return _enumType; }
            set
            {
                if (value != _enumType)
                {
                    if (null != value)
                    {
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;

                        if (!enumType.IsEnum)
                            throw new ArgumentException("Type must be for an Enum.");
                    }

                    _enumType = value;
                }
            }
        }

        public EnumBindingSourceExtension() { }

        public EnumBindingSourceExtension(Type enumType)
        {
            EnumType = enumType;
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {

            if (null == _enumType)
                throw new InvalidOperationException("The EnumType must be specified.");

            Type actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;

            Array enumValues = actualEnumType.GetFields(BindingFlags.Public | BindingFlags.Static).Select(x => ((DescriptionAttribute)x.GetCustomAttributes(typeof(DescriptionAttribute), false)[0]).Description).ToArray();


            if (actualEnumType == _enumType)
                return enumValues;

            Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }
}