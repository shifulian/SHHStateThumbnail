using SHH.UI.Curve.Core;
using SHH.UI.Curve.Thumbnail;
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

namespace SHH.UI.Curve.Thumbnail.Behaviors
{
    /// <summary>
    /// 拖动行为
    /// </summary>
    [TemplatePart(Name = "dragBorder", Type = typeof(Border))]
    public class DragInCanvasBehavior : Behavior<DragContentControl>
    {
        //容器
        private Canvas canvas;
        //拖动时鼠标的偏移
        private Point mouseOffset;
        //是否处于拖动状态
        private bool isDragging = false;
        //拖动边框
        private Border dragBorder;

        //滑块移动事件
        public event Action<Position> OnDragBorderMove;

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

        /// <summary>
        /// 获取模版中的控件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected T GetTemplateControl<T>(String name) where T : DependencyObject
        {
            return AssociatedObject.Template.FindName(name, AssociatedObject) as T;
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

            this.OnDragBorderMove += AssociatedObject.DragBorderMove;
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

            this.OnDragBorderMove -= AssociatedObject.DragBorderMove;
        }


        /// <summary>
        /// 鼠标左键点击控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)e.OriginalSource;
            if (element.Name == "dragBorder")
            {
                isDragging = true;
                mouseOffset = e.GetPosition(AssociatedObject);
                element.CaptureMouse();
            }
        }

        /// <summary>
        /// 拖动控件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseMove(object sender, MouseEventArgs e)
        {
            if (canvas == null || dragBorder == null)
            {
                canvas = (Canvas)VisualTreeHelper.GetParent(AssociatedObject);
                dragBorder = GetTemplateControl<Border>("dragBorder");
            }

            FrameworkElement element = (FrameworkElement)e.OriginalSource;
            if (element.Name == "dragBorder")
            {
                if (!isDragging)
                    return;
                Point point = e.GetPosition(canvas);
                Left = point.X - mouseOffset.X;
            }
        }

        /// <summary>
        /// 拖动控件时左键松开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)e.OriginalSource;

            if (element.Name == "dragBorder")
            {
                if (!isDragging)
                    return;
                isDragging = false;
                element.ReleaseMouseCapture();
            }

            //计算选中区域的位置
            Position p = new Position
            {
                p1 = Left / canvas.ActualWidth,
                p2 = (Left + dragBorder.ActualWidth) / canvas.ActualWidth
            };

            //触发滑块移动事件
            OnDragBorderMove?.Invoke(p);
        }


        #endregion
    }
}
