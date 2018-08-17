using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using SHH.UI.Curve.Core;
using SHH.UI.Curve.Pub;

namespace SHH.UI.Curve.Detail
{
    /// <summary>
    /// 曲线图X轴
    /// </summary>
    public class SHHCurveDetailAxisX : SHHContentControl
    {
        //容器
        private UniformGrid uniformGrid;
        //标签个数
        private int count = 12;
        //X轴缩放比例
        private double scaleRatioAxisX = 1;
        //开始时间
        private DateTime startTime;
        //结束时间
        private DateTime endTime;
        //时间间隔
        private TimeSpan timeSpan;
        //时间区间
        private TimeInterval timeInterval;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SHHCurveDetailAxisX()
        {
            LoadStyleFromAssembly("SHHStateDetail;component/Style/SHHCurveDetailAxisXStyle.xaml", UriKind.Relative, "SHHCurveDetailAxisXStyle");
        }

        /// <summary>
        /// 应用模版时触发
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            uniformGrid = GetTemplateChild<UniformGrid>("uniformGrid");
        }

        /// <summary>
        /// 改变X轴
        /// </summary>
        /// <param name="timeInterval"></param>
        public void UpdateAxisX()
        {
            //更新时间区间
            timeSpan = timeInterval.EndTime - timeInterval.StartTime;
            startTime = timeInterval.StartTime;
            endTime = startTime + new TimeSpan((long)(timeSpan.Ticks / scaleRatioAxisX));

            uniformGrid.Children.Clear();
            uniformGrid.Children.Add(new SHHCurveDetailAxisXLabel { Text = startTime.ToString("yy/M/d h:mm:ss", DateTimeFormatInfo.InvariantInfo), HorizontalAlignment = HorizontalAlignment.Left });
            for (int i = 1; i < Count - 1; ++i)
            {
                uniformGrid.Children.Add(new SHHCurveDetailAxisXLabel { Text = (startTime + new TimeSpan((long)(timeSpan.Ticks / (Count - 1.0) * i / scaleRatioAxisX))).ToString("M/d h:mm:ss", DateTimeFormatInfo.InvariantInfo) });
            }
            uniformGrid.Children.Add(new SHHCurveDetailAxisXLabel { Text = endTime.ToString("yy/M/d h:mm:ss", DateTimeFormatInfo.InvariantInfo), HorizontalAlignment = HorizontalAlignment.Right });
        }


        /// <summary>
        /// 标签个数
        /// </summary>
        public int Count { get => count; set => count = value; }

        /// <summary>
        /// X轴缩放比例
        /// </summary>
        public double ScaleRatioAxisX
        {
            get => scaleRatioAxisX;
            set
            {
                //X轴缩放不能小于1
                if (value < 1)
                    value = 1;
                scaleRatioAxisX = value;
            }
        }

        /// <summary>
        /// 时间区间
        /// </summary>
        public TimeInterval TimeInterval { get => timeInterval; set => timeInterval = value; }
    }
}
