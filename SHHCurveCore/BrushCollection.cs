using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SHH.CurvePage.Model
{
    /// <summary>
    /// 画刷集合
    /// </summary>
    public class BrushCollection
    {
        /// <summary>
        /// 画刷颜色集合
        /// </summary>
        public static SolidColorBrush[] Brushes = new SolidColorBrush[]
        {
            new SolidColorBrush(Color.FromRgb(79, 188, 195)),
            new SolidColorBrush(Color.FromRgb(141, 222, 166)),
            new SolidColorBrush(Color.FromRgb(186, 233, 93)),
            new SolidColorBrush(Color.FromRgb(97, 198, 58)),
            new SolidColorBrush(Color.FromRgb(190, 185, 226)),
            new SolidColorBrush(Color.FromRgb(130, 181, 247)),
            new SolidColorBrush(Color.FromRgb(160, 245, 222)),
            new SolidColorBrush(Color.FromRgb(169, 249, 186)),
            new SolidColorBrush(Color.FromRgb(184, 222, 225)),
            new SolidColorBrush(Color.FromRgb(247, 190, 222)),
            new SolidColorBrush(Color.FromRgb(225, 161, 247)),
            new SolidColorBrush(Color.FromRgb(87, 77, 202)),
            new SolidColorBrush(Color.FromRgb(97, 212, 209)),
            new SolidColorBrush(Color.FromRgb(190, 183, 190)),
            new SolidColorBrush(Color.FromRgb(168, 189, 58)),
            new SolidColorBrush(Color.FromRgb(241, 220, 165)),
        };

        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public SolidColorBrush this[int index]
        {
            get
            {
                if (index < Brushes.Count())
                    return Brushes[index];
                return new SolidColorBrush(Color.FromRgb((byte)new Random(Guid.NewGuid().GetHashCode()).Next(0,255), (byte)new Random(Guid.NewGuid().GetHashCode()).Next(0, 255), (byte)new Random(Guid.NewGuid().GetHashCode()).Next(0, 255)));
            }
        }


        /// <summary>
        /// 集合总数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Brushes.Count();
        }
    }
}
