using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bonsai_rx_xplat
{
    public class PropertyUtils
    {
        public static dynamic Cast(dynamic obj, Type castTo)
        {
            return Convert.ChangeType(obj, castTo);
        }
    }

    // Wrapper class that allows XAML to get and set reflected properties by exposing them as getters / commands that interact with PropertyInfo
    public class PropertyData
    {
        public PropertyInfo PropertyInformation { get; set; }
        public object Target { get; set; }
        public static int test;
        public object TargetPropertyValue
        {
            get
            {
                return PropertyInformation.GetValue(Target);
            }
        }
        public Command SetTargetPropertyValue { get; private set; }

        public PropertyData()
        {

            SetTargetPropertyValue = new Command(obj =>
            {
                var value = ((Entry)obj).Text;
                var dType = PropertyInformation.PropertyType;

                var converter = TypeDescriptor.GetConverter(dType);
                var converted = converter.ConvertFromString(value);

                PropertyInformation.SetValue(Target, converted);
            });
        }
    }
}
