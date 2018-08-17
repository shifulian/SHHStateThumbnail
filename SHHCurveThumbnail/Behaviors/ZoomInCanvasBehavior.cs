using System;
using System.Collections.Generic;
using System.Windows.Shapes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows;
using SHH.UI.Curve.Core;

namespace SHH.UI.Curve.Thumbnail.Behaviors
{
    /// <summary>
    /// 缩放行为
    /// </summary>
    [TemplatePart(Name = "rightPath", Type = typeof(Border))]
    [TemplatePart(Name = "leftPath", Type = typeof(Border))]
    public class ZoomInCanvasBehavior : Behavior<DragContentControl>
    {
        //左边的三角
        private Path leftPath;
        //右边的三角
        private Path rightPath;
        //容器
        private Canvas canvas;
        //是否处于拖动状态
        private bool isDragging = false;

        //滑块移动事件
        public event Action<Position> OnDragBorderMove;



        /// <summary>
        /// 宽度
        /// </summary>
        private double width;
        public double Width
        {
            get => AssociatedObject.ActualWidth;
            set
            {
                if (value >= AssociatedObject.MinWidth && value + Left <= canvas.ActualWidth)
                {
                    width = value;
                    AssociatedObject.Width = value;
                }
            }
        }

        /// <summary>
        /// 左边的距离
        /// </summary>
        private double left;
        public double Left
        {
            get => (double)AssociatedObject.GetValue(Canvas.LeftProperty);
            set
            {
                if (value >= 0 && value + Width <= canvas.ActualWidth)
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

            //这个事件在DragInCanvasBehavior加过了
            //this.OnDragBorderMove += AssociatedObject.DragBorderMove;
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
        }

        /// <summary>
        /// 鼠标左键按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)e.OriginalSource;
            if (element.Name.Contains("Path"))
            {
                isDragging = true;
                element.CaptureMouse();
            }
        }

        /// <summary>
        /// 鼠标左键松开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)e.OriginalSource;

            if (element.Name.Contains("Path"))
            {
                if (!isDragging)
                    return;
                isDragging = false;
                element.ReleaseMouseCapture();

                //计算选中区域的位置
                Position p = new Position
                {
                    p1 = Left / canvas.ActualWidth,
                    p2 = (Left + AssociatedObject.ActualWidth) / canvas.ActualWidth
                };

                //触发滑块移动事件
                OnDragBorderMove?.Invoke(p);
            }
        }

        /// <summary>
        /// 鼠标移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MouseMove(object sender, MouseEventArgs e)
        {

            if (leftPath == null || rightPath == null || canvas == null)
            {
                leftPath = GetTemplateControl<Path>("leftPath");
                rightPath = GetTemplateControl<Path>("rightPath");
                canvas = (Canvas)VisualTreeHelper.GetParent(AssociatedObject);
            }

            if (!isDragging)
                return;

            Point point = e.GetPosition(canvas);
            FrameworkElement element = (FrameworkElement)e.OriginalSource;
            if (element.Name == "rightPath")
            {
                double temp = point.X - Left;

                //拉升到最小时向左滑动
                if (temp < AssociatedObject.MinWidth)
                {
                    Left -= 1;
                    Width -= 1;
                }
                else
                {
                    Width = temp;
                }
            }
            else if (element.Name == "leftPath")
            {
                double temp = Left - point.X;

                //防止超出左边界限
                if (Left - temp < 0)
                    return;

                ////防止到达最小宽度后,向右偏移
                //if (Width + temp < 30)
                //    return;

                //当拉伸到最大时向右缩放可能会导致Width溢出,只能限制成-1
                if (Left + Width - temp > canvas.ActualWidth)
                {
                    temp = -1;
                }

                Left -= temp;
                Width += temp;

                //打印位置信息
                //Console.WriteLine(Left + " " + Width + " " + temp);
            }
        }
        #endregion
    }
}
