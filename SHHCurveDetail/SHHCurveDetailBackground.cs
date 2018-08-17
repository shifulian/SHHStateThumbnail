using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SHH.UI.Curve.Core;
using SHH.UI.Curve.Pub;

namespace SHH.UI.Curve.Detail
{
    /// <summary>
    /// 曲线图背景
    /// </summary>
    [TemplatePart(Name = "legend", Type = typeof(SHHCurveDetailLegend))]
    [TemplatePart(Name = "axisX", Type = typeof(SHHCurveDetailAxisX))]
    [TemplatePart(Name = "axisY", Type = typeof(SHHCurveDetailAxisY))]
    public class SHHCurveDetailBackground : SHHContentControl
    {
        //图例
        private SHHCurveDetailLegend legend;
        //X轴
        private SHHCurveDetailAxisX axisX;
        //Y轴
        private SHHCurveDetailAxisY axisY;


        /// <summary>
        /// 构造函数
        /// </summary>
        public SHHCurveDetailBackground()
        {
            LoadStyleFromAssembly("SHHStateDetail;component/Style/SHHCurveDetailBackgroundStyle.xaml", UriKind.Relative, "SHHCurveDetailBackgroundStyle");
        }

        /// <summary>
        /// 改变曲线图背景
        /// </summary>
        /// <param name="timeInterval"></param>
        public void UpdateBackground()
        {
            //改变图例
            legend.UpdateLegend();
            //改变X轴
            axisX.UpdateAxisX();
            //改变Y轴
            axisY.UpdateAxisY();
        }

        /// <summary>
        /// 应用模版时触发
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            legend = GetTemplateChild<SHHCurveDetailLegend>("legend");
            axisX = GetTemplateChild<SHHCurveDetailAxisX>("axisX");
            axisY = GetTemplateChild<SHHCurveDetailAxisY>("axisY");
        }

        /// <summary>
        /// X轴标签个数
        /// </summary>
        public int XCount { get => axisX.Count; set => axisX.Count = value; }

        /// <summary>
        /// Y轴标签个数
        /// </summary>
        public int YCount { get => axisY.Count; set => axisY.Count = value; }

        /// <summary>
        /// X轴缩放比例
        /// </summary>
        public double ScaleRatioAxisX
        {
            get => axisX.ScaleRatioAxisX;
            set
            {
                //X轴缩放比例必须大于1
                if (value < 1)
                    value = 1;
                axisX.ScaleRatioAxisX = value;
            }
        }

        /// <summary>
        /// Y轴缩放比例
        /// </summary>
        public double ScaleRatioAxisY
        {
            get => axisY.ScaleRatioAxisY;
            set
            {
                axisY.ScaleRatioAxisY = value;
            }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public double MaxValue { get => axisY.MaxValue; set => axisY.MaxValue = value; }

        /// <summary>
        /// 最小值
        /// </summary>
        public double MinValue { get => axisY.MinValue; set => axisY.MinValue = value; }

        /// <summary>
        /// 曲线集合
        /// </summary>
        public List<BrokenLine> BrokenLines { get => legend.BrokenLines; set => legend.BrokenLines = value; }

        /// <summary>
        /// 时间区间
        /// </summary>
        public TimeInterval TimeInterval { get => axisX.TimeInterval; set => axisX.TimeInterval = value; }
    }
}
