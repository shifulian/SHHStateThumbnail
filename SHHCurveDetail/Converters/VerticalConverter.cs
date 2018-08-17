using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SHH.UI.Curve.Detail.Converters
{
    /// <summary>
    /// 垂直位置的转换器,用于处理X轴标签的位置
    /// </summary>
    public class VerticalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ContentControl control = ((TextBlock)value).TemplatedParent as ContentControl;

            if (control.Parent is Panel panel)
            {
                for (int i = 0; i < panel.Children.Count; ++i)
                {
                    if (panel.Children[i] == control)
                    {
                        if ((i + 2) % 2 == 0)
                        {
                            return VerticalAlignment.Top;
                        }
                        else
                        {
                            return VerticalAlignment.Bottom;
                        }
                    }
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
