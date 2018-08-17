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
    /// 曲线图图例
    /// </summary>
    public class SHHCurveDetailLegend : SHHContentControl
    {
        //堆栈面板
        private StackPanel stackPanel;
        //曲线集合
        private List<BrokenLine> brokenLines;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SHHCurveDetailLegend()
        {
            LoadStyleFromAssembly("SHHStateDetail;component/Style/SHHCurveDetailLegendStyle.xaml", UriKind.Relative, "SHHCurveDetailLegendStyle");
        }

        /// <summary>
        /// 应用模版时触发
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            stackPanel = GetTemplateChild<StackPanel>("stackPanel");
        }


        /// <summary>
        /// 改变图例
        /// </summary>
        /// <param name="lines"></param>
        public void UpdateLegend()
        {
            stackPanel.Children.Clear();

            bool flag = false;

            for (int i = 0; i < BrokenLines.Count; ++i)
            {
                LegendToggleButton button = new LegendToggleButton
                {
                    IsChecked = flag,
                    //BorderBrush = BrokenLines[i].Brush,
                    Content = BrokenLines[i].Name
                };

                stackPanel.Children.Add(button);

                flag = !flag;
            }
        }

        /// <summary>
        /// 曲线集合
        /// </summary>
        public List<BrokenLine> BrokenLines { get => brokenLines; set => brokenLines = value; }
    }
}
