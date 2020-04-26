using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Sequence.Test.UI
{
    public class MethodInvokeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null || !(parameter is string)) return null;
            if (value == null) return null;
            var name = parameter as string;
            var method = value.GetType().GetMethod(name);
            if (method == null) return null;
            return method.Invoke(value, new object[] { });
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
