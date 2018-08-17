using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace SHH.UI.Curve.Detail.Behaviors
{
    /// <summary>
    /// 标尺跳转行为
    /// </summary>
    public class RulerJumpBehavior : Behavior<RulerContentControl>
    {
        //容器
        private Canvas canvas;

        /// <summary>
        /// 左边的距离
        /// </summary>
        private double left;
        public double Left
        {
            get => (double)AssociatedObject.GetValue(Canvas.LeftProperty);
            set
            {
                if (value >= 0 && value + AssociatedObject.RenderSize.Width <= canvas.ActualWidth)
                {
                    left = value;
                    AssociatedObject.SetValue(Canvas.LeftProperty, value);
                }
            }
        }

        #region 事件
        /// <summary>
        /// 控件初始化完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load(object sender, RoutedEventArgs e)
        {
            if (canvas == null)
            {
                canvas = (Canvas)VisualTreeHelper.GetParent(AssociatedObject);
            }
            canvas.MouseLeftButtonDown += MouseDown;
        }

        /// <summary>
        /// 鼠标按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point p = e.GetPosition(canvas);
            Left = p.X;
            AssociatedObject.RulerMove(Left / canvas.ActualWidth);
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += Load;
        }


        /// <summary>
        /// 移除事件
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Loaded -= Load;
        }

        #endregion
    }
}
