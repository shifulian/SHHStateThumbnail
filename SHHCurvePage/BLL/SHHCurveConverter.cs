using SHH.CurvePage.BLL;
using SHH.UI.Curve.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using SHH.CurvePage.Model;

namespace SHH.CurvePage.BLL
{
    /// <summary>
    /// 状态数据转换器类
    /// </summary>
    public class SHHCurveConverter
    {
        //曲线名:曲线区间的开始时间和结束时间
        private Dictionary<String, TimeInterval> timeIntervalDictionary = new Dictionary<string, TimeInterval>();
        //单位时间间隔
        private long timeSpanTick;

        private DateTime startTime;
        private DateTime endTime;



        /// <summary>
        /// Dt转成List
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<BrokenLine> ConvertDtToList(DataTable dt, DateTime startTime, DateTime endTime, TimeSpan timeSpan)
        {
            if (dt.Rows.Count == 0)
                return null;

            this.startTime = startTime;
            this.endTime = endTime;

            #region
            //再加一个循环获取多组数据


            //List<BrokenLine> lines = new List<BrokenLine>
            //{
            //    new BrokenLine(new List<StatePoint>(), 10, "aaa")
            //}; 

            //for (int j = 0; j < lines.Count; ++j)
            //{

            //    //开始的状态
            //    lines[j].Points.Add(new StatePoint() { state = (int)dt.Rows[0]["switchStateNum"], point = new Point(0, 0) });

            //    for (int i = 1; i < dt.Rows.Count; ++i)
            //    {
            //        //和上一时刻的状态不同
            //        if ((int)dt.Rows[i]["switchStateNum"] != (int)dt.Rows[i - 1]["switchStateNum"])
            //        {
            //            lines[j].Points.Add(new StatePoint() { state = (int)dt.Rows[i]["switchStateNum"], point = new Point(((DateTime)dt.Rows[i]["startTime"] - startTime).Ticks / ((endTime - startTime).Ticks + 0.0), 0) });
            //        }
            //    }
            //    lines[j].Points.Add(new StatePoint() { state = (int)dt.Rows[0]["switchStateNum"], point = new Point(1, 0) });

            //}
            #endregion


            ////获取最大最小值
            //GetMaxMinValue(dt);
            ////获取曲线
            List<BrokenLine> lines = GetBrokeLines(dt);

            ////X轴缩放,缺少的地方用零填充
            lines = GetStretchLines(lines, timeSpan);

            ////获取各曲线时间区间
            //GetDateTimeInterval(dt);

            ////X轴填充(填充曲线)
            lines = FillCurve(lines);

            //返回曲线集合
            return lines;
        }

        #region 【获取折线】
        /// <summary>
        /// 获取折线
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<BrokenLine> GetBrokeLines(DataTable dt)
        {
            //折线集合
            List<BrokenLine> lines = new List<BrokenLine>();

            Dictionary<string, BrokenLine> dictionary = new Dictionary<string, BrokenLine>();

            Dictionary<int, BrokenLine> dictionarys = new Dictionary<int, BrokenLine>();

            //遍历线条
            for (int i = 0; i < lines.Count; ++i)
            {

                List<StatePoint> points = new List<StatePoint>();


                for (int j = 0; j < lines[i].Points.Count(); ++j)
                {
                    points.Add(j);
                }
                dictionarys.Add(points, new BrokenLine(new List<StatePoint>()) { Points = points });

            }

            for (int i = 1; i < dt.Rows.Count; ++i)
            {
                //非正常状态
                if (dt.Rows[i]["switchStateNum"].ToString() != "0")
                {
                    continue;
                }

                //如果不存在该测点,则添加
                String pointName = dt.Rows[i]["testPoint"].ToString();

                //如果字典中不存在该测点
                if (!dictionary.ContainsKey(pointName))
                {
                    dictionary.Add(pointName, new BrokenLine(new List<StatePoint>()) { Name = pointName });
                }

                //和上一时刻的状态不同
                if ((int)dt.Rows[i]["switchStateNum"] != (int)dt.Rows[i - 1]["switchStateNum"])
                {
                    dictionary[pointName].Points.Add(new StatePoint() { state = (int)dt.Rows[i]["switchStateNum"], point = new Point(((DateTime)dt.Rows[i]["startTime"] - startTime).Ticks / ((endTime - startTime).Ticks + 0.0), 0) });

                }
            }
            foreach (var i in dictionary)
            {
                lines.Add(i.Value);
            }
            return lines;


            #region  
            ////折线集合
            //List<BrokenLine> lines = new List<BrokenLine>();
            //////最小值点集合
            ////List<Point> pointsMin = new List<Point>();
            //////平均值点集合
            ////List<Point> pointsAvg = new List<Point>();
            //////最大值点集合
            ////List<Point> pointsMax = new List<Point>();

            ////包含曲线名称和曲线对象的字典(一个测点包含三条曲线)
            //Dictionary<String, BrokenLine> dictionary = new Dictionary<String, BrokenLine>();

            //for (int i = 0; i < dt.Rows.Count; ++i)
            //{
            //    //非正常状态
            //    if (dt.Rows[i]["switchStateNum"].ToString() != "0")
            //    {
            //        continue;
            //    }

            //    //如果不存在该测点,则添加
            //    String pointName = dt.Rows[i]["testPoint"].ToString();

            //    //如果字典中不存在该测点
            //    if (!dictionary.ContainsKey(pointName))
            //    {
            //        //dictionary.Add(pointName, new BrokenLine(new List<StatePoint>()) { Name = pointName });
            //    }

            //    String valueStr = dt.Rows[i]["difTime"].ToString();

            //    //转换成double
            //    if (!double.TryParse(valueStr, out double value))
            //        value = 0;

            //    DateTime time = Convert.ToDateTime(dt.Rows[i]["countTime"]);


            //    // dictionary[pointName].Points.Add(value / (SHHCurveManager.Max - SHHCurveManager.Min));
            //dictionary[pointName].Points.Add(new Point((time - startTime).Ticks / ((endTime - startTime).Ticks + 0.0), value / (SHHCurveManager.Max - SHHCurveManager.Min)));

            //}

            //foreach (var i in dictionary)
            //{
            //    lines.Add(i.Value);
            //}


            ////返回曲线对象集合
            //return lines;
            #endregion

        }
        #endregion

        #region 【 填充曲线,设置颜色,线帽类型等】
        /// <summary>
        /// 填充曲线,设置颜色,线帽类型等
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        private List<BrokenLine> FillCurve(List<BrokenLine> lines)
        {
            for (int i = 0; i < lines.Count; ++i)
            {
                //lines[i].Name = lines[i].Name;
                //lines[i].Brush = GetBrush(i);
                lines[i].Thickness = 8;
                //lines[i].BrokenLineCapType = GetBrokenLineCapType(i);
            }
            return lines;

            #region 之前的逻辑
            //List<BrokenLine> newLines = new List<BrokenLine>();
            //for (int i = 0; i < lines.Count; ++i)
            //{
            //    //X轴坐标个数
            //    long xCount = (SHHCurveManager.EndTime - SHHCurveManager.StartTime).Ticks / timeSpanTick + 1;
            //    List<Point> points = new List<Point>();

            //    for (int j = 0, k = 0; j < xCount; ++j)
            //    {
            //        //判断位置
            //        if (SHHCurveManager.StartTime + new TimeSpan(j * timeSpanTick) >
            //            timeIntervalDictionary[lines[i].Name.Split(' ')[0]].StartTime)
            //        {
            //            if (k < lines[i].Points.Count)
            //            {
            //                points.Add(new Point(j / (xCount + 0.0), lines[i].Points[k].Y));
            //                ++k;
            //            }
            //            else
            //            {
            //                //右边区间填充0
            //                points.Add(new Point(j / (xCount + 0.0), 0));
            //            }
            //        }
            //        else
            //        {
            //            //左边区间填充0
            //            points.Add(new Point(j / (xCount + 0.0), 0));
            //        }
            //    }

            //    newLines.Add(new BrokenLine(points));

            //    //设置名字,颜色,宽度,线帽类型
            //    newLines[i].Name = lines[i].Name;
            //    newLines[i].Brush = GetBrush(i);
            //    newLines[i].Thickness = 1;
            //    newLines[i].BrokenLineCapType = GetBrokenLineCapType(i);
            //}

            //return newLines;
            #endregion
        }
        #endregion

        #region 【获取各曲线时间区间】
        /// <summary>
        /// 获取各曲线时间区间
        /// </summary>
        /// <param name="dt"></param>
        private void GetDateTimeInterval(DataTable dt)
        {
            //计算单位时间间隔
            timeSpanTick = (Convert.ToDateTime(dt.Rows[dt.Rows.Count - 1]["countTime"]) - Convert.ToDateTime(dt.Rows[0]["countTime"])).Ticks / dt.Rows.Count;

            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                //如果不存在该测点,则添加
                String pointName = dt.Rows[i]["testPoint"].ToString();
                if (!timeIntervalDictionary.ContainsKey(pointName))
                {
                    timeIntervalDictionary.Add(pointName, new TimeInterval(Convert.ToDateTime(dt.Rows[i]["countTime"].ToString()), Convert.ToDateTime(dt.Rows[i]["countTime"].ToString())));
                }
                else
                {
                    DateTime time = Convert.ToDateTime(dt.Rows[i]["countTime"].ToString());
                    if (time > timeIntervalDictionary[pointName].EndTime)
                    {
                        timeIntervalDictionary[pointName].EndTime = time;
                    }
                }
            }
        }
        #endregion

        #region 缩放曲线的X轴,缺少的地方用零填充
        /// <summary>
        /// 缩放曲线的X轴,缺少的地方用零填充
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>i代表数据组数</returns>
        private List<BrokenLine> GetStretchLines(List<BrokenLine> lines, TimeSpan timeSpan)
        {
            //List<BrokenLine> newLines = new List<BrokenLine>();


            for (int j = 0; j < lines.Count; j++)
            {
                int i = 0;
                List<StatePoint> newPoints = new List<StatePoint>();

                for (DateTime time = startTime; time <= endTime; time += timeSpan)
                {
                    //当前位置百分比
                    double p = (time - startTime).Ticks / ((endTime - startTime).Ticks + 0.0);

                    if (i < lines[j].Points.Count && p >= lines[j].Points[i].point.X)
                    {
                        //newPoints.Add(new Point(p, lines[j].Points[i].Y));
                        newPoints.Add(new StatePoint() { state = newPoints.Count, point = new Point(p, lines[j].Points[i].point.Y) });
                        ++i;
                    }
                    else
                    {
                        //当前时刻没有值,用0填充
                        //newPoints.Add(new Point(p, 0));
                        newPoints.Add(new StatePoint() { state = newPoints.Count, point = new Point(p, 0) });
                    }
                }
                lines[j].Points = newPoints;
            }
            return lines;


            #region
            //for (int j = 0; j < lines.Count; ++j)
            //{
            //    int i = 0;
            //    List<Double> newPoints = new List<Double>();
            //    for (DateTime time = startTime; time <= endTime; time += timeSpan)
            //    {
            //        //当前位置百分比
            //        double p = (time - startTime).Ticks / ((endTime - startTime).Ticks + 0.0);

            //        if (i < lines[j].Points.Count && p >= lines[j].Points[i].X)
            //        {
            //            newPoints.Add(new Point(p, lines[j].Points[i].Y));
            //            ++i;
            //        }
            //        else
            //        {
            //            //当前时刻没有值,用0填充
            //            newPoints.Add(new Point(p, 0));
            //        }

            //    }
            //    lines[j].Points = newPoints;
            //}
            //return lines;


            //for (int j = 0; j < lines.Count; ++j)
            //{
            //    //曲线的时间区间
            //    DateTime startTime = timeIntervalDictionary[lines[j].Name.Split(' ')[0]].StartTime;
            //    DateTime endTime = timeIntervalDictionary[lines[j].Name.Split(' ')[0]].EndTime;

            //    for (int i = 0; i < lines[j].Points.Count; ++i)
            //    {
            //        lines[j].Points[i] = new Point(
            //            i / (lines[j].Points.Count - 1 + 0.0),
            //            lines[j].Points[i].Y);
            //    }
            //}
            //return lines;
            #endregion
        }
        #endregion

        #region 【获取线帽类型】
        /// <summary>
        /// 获取线帽类型
        /// </summary>
        /// <param name="j"></param>
        /// <returns></returns>
        private BrokenLineCapType GetBrokenLineCapType(int j)
        {
            return SHHCurveManager.BrokenLineCapTypes[(j + SHHCurveManager.BrokenLineCapTypes.Count()) % SHHCurveManager.BrokenLineCapTypes.Count()];
        }
        #endregion

        #region 【获取画刷】
        /// <summary>
        /// 获取画刷
        /// </summary>
        /// <param name="j"></param>
        /// <returns></returns>
        private Brush GetBrush(int j)
        {
            return SHHCurveManager.BrushCollection[(j + SHHCurveManager.BrushCollection.Count()) % SHHCurveManager.BrushCollection.Count()];
        }
        #endregion



        #region 【获取最大最小值】
        /// <summary>
        /// 获取最大最小值
        /// </summary>
        /// <param name = "dt" ></ param >
        private void GetMaxMinValue(DataTable dt)
        {
            //double min = 0;
            //double max = -1;

            ////遍历集合dt,获取最大值
            //for (int i = 0; i < dt.Rows.Count; ++i)
            //{
            //    if (dt.Rows[i]["switchStateNum"].ToString() != "0")
            //    {
            //        continue;
            //    }

            //    Double.TryParse(dt.Rows[i]["difTime"].ToString(), out double resultMax);
            //    max = (resultMax > max) ? resultMax : max;
            //}

            ////没有获取到最大最小值
            //if (max == 0 && min == 0)
            //{
            //    max = 1;
            //    min = 0;
            //}

            //SHHCurveManager.Max = max;
            //SHHCurveManager.Min = min;
        }
        #endregion

        #region 【String格式的XML转Dt】
        /// <summary>
        /// String格式的XML转Dt
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Obsolete("弃用", false)]
        public DataSet ConvertXMLToDataSet(string data)
        {
            using (StringReader stream = new StringReader(data))
            {
                using (XmlTextReader reader = new XmlTextReader(stream))
                {
                    try
                    {
                        DataSet ds = new DataSet();
                        ds.ReadXml(reader);
                        return ds;
                    }
                    catch
                    {
                        return null;
                    }
                }
            }
        }
        #endregion
    }
}
