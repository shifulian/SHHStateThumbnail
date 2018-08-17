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
    /// 标尺拖动行为
    /// </summary>
    public class RulerDragBehavior : Behavior<RulerContentControl>
    {
        //容器
        private Canvas canvas;
        //拖动时鼠标的偏移
        private Point mouseOffset;
        //是否处于拖动状态
        private bool isDragging = false;

        //标尺移动事件
        public event Action<double> OnRulerMove;

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
        /// 添加事件
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.MouseLeftButtonDown += MouseLeftButtonDown;
            AssociatedObject.MouseMove += MouseMove;
            AssociatedObject.MouseLeftButtonUp += MouseLeftButtonUp;

            OnRulerMove += AssociatedObject.RulerMove;
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.MouseLeftButtonDown -= MouseLeftButtonDown;
            AssociatedObject.MouseMove -= MouseMove;
            AssociatedObject.MouseLeftButtonUp -= MouseLeftButtonUp;

            OnRulerMove -= AssociatedObject.RulerMove;
        }


        /// <summary>
        /// 鼠标左键点击控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)e.OriginalSource;
            isDragging = true;
            mouseOffset = e.GetPosition(AssociatedObject);
            element.CaptureMouse();
        }

        /// <summary>
        /// 拖动控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (canvas == null)
            {
                canvas = (Canvas)VisualTreeHelper.GetParent(AssociatedObject);
            }

            if (!isDragging)
                return;
            Point point = e.GetPosition(canvas);
            Left = point.X - mouseOffset.X;

            //触发标尺移动事件
            OnRulerMove?.Invoke(Left / canvas.ActualWidth);
        }

        /// <summary>
        /// 拖动控件时左键松开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)e.OriginalSource;
            if (!isDragging)
                return;
            isDragging = false;
            element.ReleaseMouseCapture();
        }

        #endregion
    }
}
