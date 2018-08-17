using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace SHH.UI.Curve.Core
{
    /// <summary>
    /// 事件传递的参数
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TEventArgs<T>
    {
        public TEventArgs(T t)
        {
            Data = t;
        }

        public T Data { get; set; }
    }
}
