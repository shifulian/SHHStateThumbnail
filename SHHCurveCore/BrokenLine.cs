using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace SHH.UI.Curve.Core
{
    /// <summary>
    /// 折线数据管理类
    /// </summary>
    [Serializable]
    public class BrokenLine
    {
        public BrokenLine(List<StatePoint> points) : this(points, 1, "")
        { }

        //public BrokenLine(List<StatePoint> points, Brush brush) : this(points, brush, 1, BrokenLineCapType.None, "")
        //{ }

        //public BrokenLine(List<StatePoint> points, Brush brush, double thickness) : this(points, brush, 1, BrokenLineCapType.None, "")
        //{ }

        //public BrokenLine(List<StatePoint> points, Brush brush, double thickness, BrokenLineCapType brokenLineCapType) : this(points, brush, 1, BrokenLineCapType.None, "")
        //{ }

        public BrokenLine(List<StatePoint> points, double thickness, String name)
        {
            Points = points;
            //Brush = brush;
            Thickness = thickness;
            //BrokenLineCapType = brokenLineCapType;
            Name = name;
        }

        //点集合(存放位置比例和状态)
        private List<StatePoint> points = new List<StatePoint>();
        //线条颜色
        //private String brush = "Black";
        //线条宽度
        private Double thickness = 1;
        //线帽类型
        private BrokenLineCapType brokenLineCapType = BrokenLineCapType.None;
        //线条名称
        private String name;
        //备用属性
        private object tag;


        /// <summary>
        /// 状态数据点集合
        /// </summary>
        public List<StatePoint> Points { get => points; set => points = value; }

        ///// <summary>
        ///// 画刷
        ///// </summary>
        //public Brush Brush
        //{
        //    get
        //    {
        //        //方便序列化
        //        BrushConverter brushConverter = new BrushConverter();
        //        return (Brush)brushConverter.ConvertFromString(brush);
        //    }
        //    set => brush = value.ToString();
        //}


        /// <summary>
        /// 线条宽度
        /// </summary>
        public double Thickness { get => thickness; set => thickness = value; }
        ///// <summary>
        ///// 线帽类型
        ///// </summary>
        //public BrokenLineCapType BrokenLineCapType { get => brokenLineCapType; set => brokenLineCapType = value; }
        /// <summary>
        /// 线条名称
        /// </summary>
        public string Name { get => name; set => name = value; }
        /// <summary>
        /// 备用属性
        /// </summary>
        public object Tag { get => tag; set => tag = value; }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns></returns>
        public BrokenLine DeepClone()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, this);
            ms.Position = 0;
            return (BrokenLine)bf.Deserialize(ms);
        }
    }
}




