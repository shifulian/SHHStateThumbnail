using SHH.UI.Curve.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SHH.CurvePage.Model
{
    public class BrokenLineCapTypeCollection
    {
        /// <summary>
        /// 测点线帽类型集合
        /// </summary>
        public static BrokenLineCapType[] BrokenLineCapTypes = new BrokenLineCapType[]
        {
            BrokenLineCapType.Circle,
            BrokenLineCapType.Diamond,
            BrokenLineCapType.RectangleH,
            BrokenLineCapType.RectangleV,
            BrokenLineCapType.Square,
            BrokenLineCapType.Triangle,
            BrokenLineCapType.TriangleR,
        };


        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public BrokenLineCapType this[int index]
        {
            get
            {
                if (index < BrokenLineCapTypes.Count())
                    return BrokenLineCapTypes[index];
                return BrokenLineCapTypes[0];
            }
        }

        /// <summary>
        /// 集合总数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return BrokenLineCapTypes.Count();
        }
    }
}
