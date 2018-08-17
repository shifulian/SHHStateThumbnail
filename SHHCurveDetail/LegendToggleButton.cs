using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace SHH.UI.Curve.Detail
{
    /// <summary>
    /// 图例样式
    /// </summary>
    public class LegendToggleButton : ToggleButton
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public LegendToggleButton()
        {
            //获取样式
            ResourceDictionary dictionary = new ResourceDictionary
            {
                Source = new Uri("SHHStateDetail;component/Style/LegendToggleButtonStyle.xaml", UriKind.Relative)
            };
            //应用样式
            this.SetValue(Window.StyleProperty, dictionary["LegendToggleButtonStyle"]);
        }
    }
}
