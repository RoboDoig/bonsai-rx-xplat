using CommunityToolkit.Maui.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bonsai_rx_xplat
{
    public class TouchEventArgsConverter : BaseConverterOneWay<TouchEventArgs?, object>
    {
        public override object? ConvertFrom(TouchEventArgs? value, CultureInfo? culture = null) => value switch
        {
            null => null,
            _ => value.Touches
        };
    }
}
