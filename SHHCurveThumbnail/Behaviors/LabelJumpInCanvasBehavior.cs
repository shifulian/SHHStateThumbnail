using SHH.UI.Curve.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace SHH.UI.Curve.Thumbnail.Behaviors
{
    /// <summary>
    /// 标签跳转的行为
    /// </summary>
    [TemplatePart(Name = "tbk_LeftTime", Type = typeof(TextBlock))]
    [TemplatePart(Name = "tbk_RightTime", Type = typeof(TextBlock))]
    public class LabelJumpInCanvasBehavior : Behavior<DragContentControl>
    {
        //右边的时间标签
        private TextBlock tbk_RightTime;
        //左边的用不到
        //private TextBlock tbk_LeftTime;

        //滑块移动事件
        public event Action<Position> OnDragBorderMove;

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
        /// 拖动控件载入时获取到tbk_RightTime对象
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Loaded(object sender, RoutedEventArgs e)
        {
            if (tbk_RightTime == null)
            {
                tbk_RightTime = GetTemplateControl<TextBlock>("tbk_RightTime");
            }
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += Loaded;
            this.OnDragBorderMove += AssociatedObject.DragBorderMove;
        }

        /// <summary>
        /// 移除事件
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Loaded -= Loaded;
            this.OnDragBorderMove -= AssociatedObject.DragBorderMove;
        }
        #endregion
    }
}
