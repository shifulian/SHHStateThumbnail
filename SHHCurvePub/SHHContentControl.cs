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
    public class SHHContentControl : ContentControl
    {
        /// <summary>
        /// 获取并程序集中的样式
        /// </summary>
        /// <param name="uriString"></param>
        /// <param name="uriKind"></param>
        /// <param name="styleKey"></param>
        protected virtual void LoadStyleFromAssembly(String uriString, UriKind uriKind, String styleKey)
        {
            //获取样式
            ResourceDictionary dictionary = new ResourceDictionary
            {
                Source = new Uri(uriString, uriKind)
            };
            //应用样式
            this.SetValue(StyleProperty, dictionary[styleKey]);
        }

        /// <summary>
        /// 获取模版内的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual T GetTemplateChild<T>(string name) where T : DependencyObject
        {
            T control = GetTemplateChild(name) as T;
            return control;
        }

        /// <summary>
        /// 创建新线程容器
        /// </summary>
        /// <param name="container">父容器</param>
        /// <param name="threadStart">子容器里执行的操作</param>
        protected virtual Thread CreateNewThreadContainer(HostVisual hostVisual, ContentControl container, ParameterizedThreadStart threadStart)
        {
            UIElement content = new VisualHost(hostVisual);
            container.Content = content;

            Thread thread = new Thread(threadStart);
            //Thread thread = new Thread(new ThreadStart(() =>
            //{
            //    VisualTarget visualTarget = new VisualTarget(hostVisual);

            //    var control = new T();
            //    control.Arrange(new Rect(new Point(), content.RenderSize));

            //    visualTarget.RootVisual = control;
            //    System.Windows.Threading.Dispatcher.Run();

            //}));

            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;

            return thread;
        }
    }
}
