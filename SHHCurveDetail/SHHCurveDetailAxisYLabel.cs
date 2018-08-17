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
    /// Y轴标签
    /// </summary>
    public class SHHCurveDetailAxisYLabel : SHHContentControl, INotifyPropertyChanged
    {
        private String text = "";


        /// <summary>
        /// 构造函数
        /// </summary>
        public SHHCurveDetailAxisYLabel()
        {
            //获取应用样式
            LoadStyleFromAssembly("SHHStateDetail;component/Style/SHHCurveDetailAxisYLabelStyle.xaml", UriKind.Relative, "SHHCurveDetailAxisYLabelStyle");
            //数据相关上下文
            DataContext = this;
        }


        /// <summary>
        /// 标签文本
        /// </summary>
        public string Text
        {
            get => text; set
            {
                text = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;
    }
}
