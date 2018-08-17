using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SHH.UI.Curve.Detail.Converters
{
    /// <summary>
    /// 时间格式转换器
    /// </summary>
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            String format="";
            if (parameter != null)
            {
                format = parameter.ToString();
            }
            return ((DateTime)value).ToString(format, DateTimeFormatInfo.InvariantInfo);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
