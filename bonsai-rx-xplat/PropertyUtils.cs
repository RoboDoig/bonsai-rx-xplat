using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace bonsai_rx_xplat
{
    public class PropertyData
    {
        public PropertyInfo PropertyInfo { get; set; }
        public object Target { get; set; }
        public object TargetPropertyValue
        {
            get
            {
                return PropertyInfo.GetValue(Target);
            }
        }
    }
}
