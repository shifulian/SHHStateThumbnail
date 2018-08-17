using SHH.UI.Curve.Pub;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SHH.UI.Curve.Detail
{
    /// <summary>
    /// X轴标签,垂直的逻辑在VerticalConverter里实现了
    /// </summary>
    public class SHHCurveDetailAxisXLabel : SHHContentControl, INotifyPropertyChanged
    {
        private String text = "";

        /// <summary>
        /// 构造函数
        /// </summary>
        public SHHCurveDetailAxisXLabel()
        {
            //获取并应用样式
            LoadStyleFromAssembly("SHHStateDetail;component/Style/SHHCurveDetailAxisXLabelStyle.xaml", UriKind.Relative, "SHHCurveDetailAxisXLabelStyle");
            //数据相关上下文
            DataContext = this;
        }

        /// <summary>
        /// 文本标签
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            }
        }

        /// <summary>
        /// 数据改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
