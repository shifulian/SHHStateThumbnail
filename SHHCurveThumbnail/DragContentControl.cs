using SHH.UI.Curve.Core;
using SHH.UI.Curve.Pub;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SHH.UI.Curve.Thumbnail
{
    /// <summary>
    /// 滑块控件
    /// </summary>
    [TemplatePart(Name = "dragBorder", Type = typeof(Border))]
    [TemplatePart(Name = "leftPath", Type = typeof(Path))]
    [TemplatePart(Name = "rightPath", Type = typeof(Path))]
    [TemplatePart(Name = "tbk_LeftTime", Type = typeof(TextBlock))]
    [TemplatePart(Name = "tbk_RightTime", Type = typeof(TextBlock))]
    public class DragContentControl : SHHContentControl, INotifyPropertyChanged
    {
        //滑块移动事件
        public event Action<Position> OnDragBorderMove;
        //属性改变事件
        public event PropertyChangedEventHandler PropertyChanged;
        //时间间隔
        private TimeInterval timeInterval = new TimeInterval();
        //右边显示的时间
        private TextBlock tbk_RightTime;
        //当前位置
        private Position position;


        /// <summary>
        /// 构造函数
        /// </summary>
        public DragContentControl()
        {
            LoadStyleFromAssembly("SHHStateThumbnail;component/Style/DragContentControlStyle.xaml", UriKind.Relative, "DragContentControlStyle");
            DataContext = this;
        }

        /// <summary>
        /// 模版应用时触发
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            tbk_RightTime = GetTemplateChild<TextBlock>("tbk_RightTime");
        }

        /// <summary>
        /// 滑块拖动事件,这里无法获取到总的时间区间,修改时间的逻辑放到上一层处理了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DragBorderMove(Position p)
        {
            position = p;
            OnDragBorderMove?.Invoke(p);
        }

        /// <summary>
        /// 左移
        /// </summary>
        public void MoveLeft()
        {
            //间隔百分比
            double x = position.p2 - position.p1;

            double newp1 = (position.p1 - x < 0) ? 0 : position.p1 - x;
            position.p2 = position.p1;
            position.p1 = newp1;

            DragBorderMove(position);
        }

        /// <summary>
        /// 改变右边时间的显示位置,0代表左边显示,1代表右
        /// </summary>
        /// <param name="i"></param>
        public void ChangeRightTimePosition(int i)
        {
            //这里距离写死了..
            if (i == 0)
            {
                tbk_RightTime.SetValue(Canvas.RightProperty, 98.0 - tbk_RightTime.ActualWidth);
            }
            else
            {
                tbk_RightTime.SetValue(Canvas.RightProperty, -98.0);
            }
        }

        /// <summary>
        /// 时间间隔
        /// </summary>
        public TimeInterval TimeInterval
        {
            get => timeInterval;
            set
            {
                timeInterval = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimeInterval"));
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get => TimeInterval.StartTime;
            set
            {
                TimeInterval.StartTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StartTime"));
            }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get => timeInterval.EndTime;
            set
            {
                TimeInterval.EndTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EndTime"));
            }
        }
    }
}
