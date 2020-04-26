using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Sequence.Test.UI
{
    public class LogMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var items = values[0] as List<LogItem>;
            var eo = values[1] as bool?;
            var ps = values[2] as bool?;
            var pf = values[3] as bool?;
            var lo = values[4] as bool?;
            if (items == null || eo == null || ps == null || pf == null || lo == null) return null;
            var query = (from i in items select i);
            if (!eo.Value)
            {
                query = query.Where(i => !i.TypeName.Equals("EventOccured"));
            }
            if (!ps.Value)
            {
                query = query.Where(i => !i.TypeName.Equals("ProcessingStarted"));
            }
            if (!pf.Value)
            {
                query = query.Where(i => !i.TypeName.Equals("ProcessingFinished"));
            }
            if (!lo.Value)
            {
                query = query.Where(i => !i.TypeName.Equals("Log"));
            }
            return query.ToList();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
