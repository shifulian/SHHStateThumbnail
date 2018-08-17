using System;

namespace SHH.UI.Curve.Core
{
    /// <summary>
    /// 时间区间
    /// </summary>
    public class TimeInterval
    {
        public TimeInterval()
        { }

        public TimeInterval(DateTime startTime,DateTime endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public DateTime StartTime = DateTime.Now;
        public DateTime EndTime = DateTime.Now;
    }
}