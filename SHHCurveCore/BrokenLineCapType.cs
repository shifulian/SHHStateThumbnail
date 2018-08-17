using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SHH.UI.Curve.Core
{
    /// <summary>
    /// 线帽类型
    /// </summary>
    [Serializable]
    public enum BrokenLineCapType
    {
        None,
        Circle,//圆
        Triangle,//三角
        TriangleR,//倒三角
        Square,//正方形
        RectangleH,//矩形水平
        RectangleV,//矩形垂直
        Diamond//菱形
    }
}

