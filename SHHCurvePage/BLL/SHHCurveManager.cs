using BLL;
using SHH.CurvePage.Model;
using SHH.UI.Curve.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SHH.CurvePage.BLL
{
    /// <summary>
    /// 状态数据管理类
    /// </summary>
    public class SHHCurveManager
    {
        //状态数据转换器
        private static SHHCurveConverter curveDataConverter = new SHHCurveConverter();
        //开始时间
        private static DateTime startTime = new DateTime(2018, 5, 1);
        //结束时间
        private static DateTime endTime = new DateTime(2018, 6, 1);
        //原始数据DataTable
        private static DataTable dt = new DataTable();
        //最大值
        private static double max;
        //最小值
        private static double min;
        //每条曲线的点数
        private static int pNum = 0;
        //曲线条数
        private static int curveCount = 0;
        //曲线BLL
        //private static HistorySimulateManager historySimulateManager = new HistorySimulateManager();
        private static HistorySwitchManager historySwithchManager = new HistorySwitchManager();
        //线帽集合
        private static BrokenLineCapTypeCollection brokenLineCapTypes = new BrokenLineCapTypeCollection();
        //画刷集合
        private static BrushCollection brushCollection = new BrushCollection();
        //单位时间间隔
        private static TimeSpan timeSpan = new TimeSpan();

        /// <summary>
        /// 更新曲线数据
        /// </summary>
        /// <returns></returns>
        public static List<BrokenLine> ReflushData()
        {
            return curveDataConverter.ConvertDtToList(Dt, StartTime, EndTime, TimeSpan);
        }

        /// <summary>
        /// 曲线数据原始数据DataTable
        /// </summary>
        public static DataTable Dt { get => dt; set => dt = value; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public static DateTime StartTime { get => startTime; set => startTime = value; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public static DateTime EndTime { get => endTime; set => endTime = value; }
      
        /// <summary>
        /// 单位时间间隔(时,分,秒)
        /// </summary>
        public static TimeSpan TimeSpan { get => timeSpan; set => timeSpan = value; }

        /// <summary>
        /// 最大值
        /// </summary>
        public static double Max { get => max; set => max = value; }

        /// <summary>
        /// 最小值
        /// </summary>
        public static double Min { get => min; set => min = value; }

        /// <summary>
        /// 一条曲线的点数
        /// </summary>
        public static int PNum { get => pNum; set => pNum = value; }

        /// <summary>
        /// 曲线条数
        /// </summary>
        public static int CurveCount { get => curveCount; set => curveCount = value; }

        /// <summary>
        /// 曲线的BLL
        /// </summary>
        //public static HistorySimulateManager HistorySimulateManager { get => historySimulateManager; set => historySimulateManager = value; }
        public static HistorySwitchManager HistorySwithchManager { get => historySwithchManager; set => historySwithchManager = value; }

        /// <summary>
        /// 线帽集合
        /// </summary>
        public static BrokenLineCapTypeCollection BrokenLineCapTypes { get => brokenLineCapTypes; set => brokenLineCapTypes = value; }

        /// <summary>
        /// 画刷集合
        /// </summary>
        public static BrushCollection BrushCollection { get => brushCollection; set => brushCollection = value; }

       
    }
}
