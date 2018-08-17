using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SHH.UI.Curve.Pub
{
    /// <summary>
    /// 曲线绘制面板
    /// </summary>
    public class DrawingCanvas : Canvas
    {
        private List<Visual> visuals = new List<Visual>();

        protected override int VisualChildrenCount => visuals.Count;

        /// <summary>
        /// 重载GetVisualChild函数,为了界面上能显示自定义的Visual对象
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        /// <summary>
        /// 添加一个Visual
        /// </summary>
        /// <param name="visual"></param>
        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);
            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }

        /// <summary>
        /// 为子控件开辟新线程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="container"></param>
        protected virtual void SetChildInNewThread<T>(Visual visual) where T : Visual
        {
            //HostVisual hostVisual = new HostVisual();
            //Thread thread = new Thread(new ThreadStart(()=> 
            //{
            //    VisualTarget target = new VisualTarget(hostVisual)
            //    {
            //        RootVisual = new VisualHost(visual)
            //    };
            //    System.Windows.Threading.Dispatcher.Run();
            //}));
            //thread.SetApartmentState(ApartmentState.STA);
            //thread.IsBackground = true;
            //thread.Start();
        }



        /// <summary>
        /// 删除Visual
        /// </summary>
        public void DeleteVisual(Visual visual)
        {
            visuals.Remove(visual);
            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }

        /// <summary>
        /// 清除所有Visual
        /// </summary>
        public void ClearVisual()
        {
            while (visuals.Count > 0)
            {
                DeleteVisual(visuals[0]);
            }
        }
    }
}
