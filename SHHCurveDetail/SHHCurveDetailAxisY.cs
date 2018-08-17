using SHH.UI.Curve.Pub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SHH.UI.Curve.Detail
{
    /// <summary>
    /// 曲线图Y轴
    /// </summary>
    [TemplatePart(Name = "maxAxisYLabel", Type = typeof(SHHCurveDetailAxisYLabel))]
    [TemplatePart(Name = "uniformGrid", Type = typeof(UniformGrid))]
    public class SHHCurveDetailAxisY : SHHContentControl
    {
        //容器
        private UniformGrid uniformGrid;
        //最大值
        private double maxValue;
        //最小值
        private double minValue;
        //标签个数
        private int count = 7;
        //Y轴缩放比例
        private double scaleRatioAxisY = 1;
        //最大值标签
        private SHHCurveDetailAxisYLabel maxAxisYLabel;
        ////最小值标签
        //private SHHCurveDetailAxisYLabel minAxisYLabel;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SHHCurveDetailAxisY()
        {
            LoadStyleFromAssembly("SHHStateDetail;component/Style/SHHCurveDetailAxisYStyle.xaml", UriKind.Relative, "SHHCurveDetailAxisYStyle");
        }

        /// <summary>
        /// 应用模版时触发
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            uniformGrid = GetTemplateChild<UniformGrid>("uniformGrid");
            maxAxisYLabel = GetTemplateChild<SHHCurveDetailAxisYLabel>("maxAxisYLabel");


            //minAxisYLabel = GetChildControl<SHHCurveDetailAxisYLabel>("minAxisYLabel") as SHHCurveDetailAxisYLabel;
        }

        /// <summary>
        /// 改变Y轴标签
        /// </summary>
        #region
        //public void UpdateAxisY()
        //{
        //    uniformGrid.Children.Clear();

        //    double min = MinValue * ScaleRatioAxisY;
        //    double max = MaxValue * ScaleRatioAxisY;


        //    maxAxisYLabel.Text = max.ToString("f2");
        //    for (int i = 1; i < Count - 1; ++i)
        //    {
        //        uniformGrid.Children.Add(new SHHCurveDetailAxisYLabel { Text = (min + (max - min) / (Count - 1.0) * (Count - 1 - i)).ToString("f2") });
        //    }
        //    uniformGrid.Children.Add(new SHHCurveDetailAxisYLabel { Text = min.ToString("f2") });
        //}
        #endregion
        public void UpdateAxisY()
        {
            uniformGrid.Children.Clear();

            double min = MinValue;
            double max = MinValue + (MaxValue - MinValue) * ScaleRatioAxisY;


            maxAxisYLabel.Text = max.ToString("f2");
            for (int i = 1; i < Count - 1; ++i)
            {
                uniformGrid.Children.Add(new SHHCurveDetailAxisYLabel { Text = (min + (max - min) / (Count - 1.0) * (Count - 1 - i)).ToString("f2") });
            }
            uniformGrid.Children.Add(new SHHCurveDetailAxisYLabel { Text = min.ToString("f2") });
        }



        /// <summary>
        /// 最大值
        /// </summary>
        public double MaxValue { get => maxValue; set => maxValue = value; }
        /// <summary>
        /// 最小值
        /// </summary>
        public double MinValue { get => minValue; set => minValue = value; }
        /// <summary>
        /// 标签总数
        /// </summary>
        public int Count { get => count; set => count = value; }
        /// <summary>
        /// Y轴缩放比例
        /// </summary>
        public double ScaleRatioAxisY { get => scaleRatioAxisY; set => scaleRatioAxisY = value; }
    }
}
