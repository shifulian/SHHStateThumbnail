using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SHH.UI.Curve.Core
{
    /// <summary>
    /// 状态数据
    /// </summary>
    [Serializable]
    public class StatePoint
    {
        public Point point = new Point();

        public int state = 0;
        //public double X;
        //public double Y;

        //public double X { get; set; }
        //public double Y { get; set; }
    }
}