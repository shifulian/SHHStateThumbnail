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
    /// 标尺控件
    /// </summary>
    [TemplatePart(Name = "tbk_Time", Type = typeof(TextBlock))]
    public class RulerContentControl : SHHContentControl, INotifyPropertyChanged
    {
        private DateTime time;
        private double scale;
        private TextBlock tbk_Time;

        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 标尺移动事件
        /// </summary>
        public event Action<double> OnRulerMove;

        /// <summary>
        /// 构造函数
        /// </summary>
        public RulerContentControl()
        {
            LoadStyleFromAssembly("SHHStateDetail;component/Style/RulerContentControlStyle.xaml", UriKind.Relative, "RulerContentControlStyle");
            DataContext = this;
        }

        /// <summary>
        /// 模版应用时触发
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            tbk_Time = GetTemplateChild<TextBlock>("tbk_Time");
        }

        /// <summary>
        /// 标尺移动
        /// </summary>
        /// <param name="scale">标尺所在的位置0-1</param>
        public void RulerMove(double scale)
        {
            this.scale = scale;

            //根据比例设置标尺的位置,硬编码
            if (scale > 0.9)
            {
                tbk_Time.SetValue(Canvas.LeftProperty, -15.0 - tbk_Time.ActualWidth);
            }
            else
            {
                tbk_Time.SetValue(Canvas.LeftProperty, 15.0);
            }

            OnRulerMove?.Invoke(scale);
        }

        /// <summary>
        /// 标尺显示的时间
        /// </summary>
        public DateTime Time
        {
            get => time;
            set
            {
                time = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
            }
        }

        /// <summary>
        /// 标尺所在位置的百分比
        /// </summary>
        public double Scale
        {
            get => scale;
        }
    }
}
