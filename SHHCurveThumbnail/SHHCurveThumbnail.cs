using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SHH.CurvePage.Model;
using SHH.UI.Curve.Core;
using SHH.UI.Curve.Pub;

namespace SHH.UI.Curve.Thumbnail
{
    /// <summary>
    /// 曲线图缩略图
    /// </summary>
    [TemplatePart(Name = "dragBorder", Type = typeof(DragContentControl))]
    [TemplatePart(Name = "drawingCanvas", Type = typeof(DrawingCanvas))]
    [TemplatePart(Name = "tbk_LeftTime", Type = typeof(TextBlock))]
    [TemplatePart(Name = "tbk_RightTime", Type = typeof(TextBlock))]
    public class SHHCurveThumbnail : SHHContentControl
    {
        //画板
        private DrawingCanvas canvas;
        //线条集合
        private List<BrokenLine> brokenLines = new List<BrokenLine>();
        //滑块
        private DragContentControl dragBorder;
        //时间间隔
        private TimeInterval timeInterval = new TimeInterval();
        //右边显示的时间
        private TextBlock tbk_RightTime;
        //当前区间位置
        //private Position position=null;

        //数据改变事件,SHHCurveDetail控件通过此事件触发数据更新
        public event Action<List<BrokenLine>, TimeInterval> OnDataChanged;

        private BrushCollection Brush = new BrushCollection();

        /// <summary>
        /// 构造函数
        /// </summary>
        public SHHCurveThumbnail()
        {
            LoadStyleFromAssembly("SHHStateThumbnail;component/Style/SHHCurveThumbnailStyle.xaml", UriKind.Relative, "SHHCurveThumbnailStyle");
            DataContext = this;
        }


        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            //绘制曲线
            DrawingLinesOnCanvas();
            if (dragBorder == null || canvas == null)
            {
                throw new Exception("Load Template Failed");
            }

            ////第一次初始化时
            //if (position == null)
            //{
            //    position = new Position() { p1 = 0, p2 = dragBorder.ActualWidth / canvas.ActualWidth };
            //}

            ////数据改变
            //DataChanged(position);

            //数据改变
            DataChanged(new Position() { p1 = 0, p2 = dragBorder.ActualWidth / canvas.ActualWidth });
        }


        #region 绘制代码
        /// <summary>
        /// 在画板上绘制折线
        /// </summary>
        public void DrawingLinesOnCanvas()
        {
            //清除之前绘制的内容
            ClearVisual();

            for (int i = 0; i < BrokenLines.Count; ++i)
            {
                //每条线作为一个visual对象
                DrawingVisual visual = new DrawingVisual();
                DrawingLinesOnVisual(visual, BrokenLines[i], i);
                canvas.AddVisual(visual);
            }
        }

        /// <summary>
        /// 在Visual中绘制折线
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="brush"></param>
        /// <param name="thickness"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="index">组数</param>
        private void DrawingLinesOnVisual(DrawingVisual visual, BrokenLine brokenLine, int index)
        {
            using (DrawingContext dc = visual.RenderOpen())
            {
                //int count = brokenLine.Points.Count;
                ////每n个绘制一次
                //int n = count / 200 + 1;

                for (int i = 1; i < brokenLine.Points.Count; i += 1)
                {
                    DrawingLine(dc,
                        brokenLine.Thickness,
                        brokenLine.Points[i - 1],
                        brokenLine.Points[i],
                        index + 1
                        );
                }

                #region
                //缩略图不需要绘制线帽
                //if (brokenLine.BrokenLineCapType != BrokenLineCapType.None)
                //{
                //    //线帽比线条的个数多一个
                //    for (int i = 0; i < brokenLine.Points.Count; i += n)
                //    {
                //        DrawLineCap(dc,
                //            brokenLine.Brush,
                //            brokenLine.Points[i], brokenLine.BrokenLineCapType);
                //    }
                //}
                #endregion
            }
        }

        /// <summary>
        /// 绘制线条
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="brush"></param>
        /// <param name="thickness"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        private void DrawingLine(DrawingContext dc, double thickness, StatePoint p1, StatePoint p2, int index)
        {
            //Point结构里存的是位置的比例,这里要做一下转换
            //Canvas的坐标原点在左上角
            Point point1 = new Point(p1.point.X * canvas.ActualWidth, canvas.ActualHeight - canvas.ActualHeight * index / (brokenLines.Count + 1.0));
            Point point2 = new Point(p2.point.X * canvas.ActualWidth, canvas.ActualHeight - (canvas.ActualHeight * index / (brokenLines.Count + 1.0) - thickness));

            dc.DrawRectangle(Brush[p1.state], new Pen(), new Rect(point1, point2));
        }
        #endregion

        /// <summary>
        /// 数据发生变化时调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataChanged(Position p)
        {
            if (OnDataChanged != null)
            {
                //筛选出的线条区间（新的线条）
                List<BrokenLine> lines = new List<BrokenLine>();

                //遍历线条
                for (int i = 0; i < brokenLines.Count; ++i)
                {
                    //筛选出状态点的值
                    List<StatePoint> points = new List<StatePoint>();
                    points.Add(new StatePoint() { state = 0, point = new Point(0, 0) });

                    //选择状态点区间
                    for (int j = (int)((brokenLines[i].Points.Count() - 1) * p.p1);
                       j < (brokenLines[i].Points.Count() - 1) * p.p2; ++j)
                    {
                        if (j == 0 || j == (brokenLines[i].Points.Count() - 1))
                            continue;

                        double value = (brokenLines[i].Points[j].point.X - p.p1) / (p.p2 - p.p1);

                        if (value < 0 || value > 1)
                            continue;

                        points.Add(new StatePoint() { state = points.Count, point = new Point(value, 0) });
                    }
                    points.Add(new StatePoint() { state = points.Count, point = new Point(1, 0) });

                    lines.Add(new BrokenLine(points, brokenLines[i].Thickness, brokenLines[i].Name));
                }

                //计算时间区间
                TimeSpan timeSpan = EndTime - StartTime;

                TimeInterval t = new TimeInterval(StartTime + new TimeSpan((long)(timeSpan.Ticks * p.p1)),
                    StartTime + new TimeSpan((long)(timeSpan.Ticks * p.p2)));
                SelectedStartTime = t.StartTime;
                SelectedEndTime = t.EndTime;

                //如果区间大于修改右边时间的显示位置,以免覆盖
                if (p.p2 > 0.85)
                {
                    //改变滑块右边的时间位置
                    dragBorder.ChangeRightTimePosition(0);
                    //改变缩略图右边的时间位置
                    tbk_RightTime.SetValue(Canvas.RightProperty, -57.0);
                }
                else
                {
                    dragBorder.ChangeRightTimePosition(1);
                    tbk_RightTime.SetValue(Canvas.RightProperty, -19.0);
                }

                //触发事件
                OnDataChanged?.Invoke(lines, t);
            }
        }


        /// <summary>
        /// 清除绘制的内容
        /// </summary>
        public void ClearVisual()
        {
            canvas.ClearVisual();
        }

        /// <summary>
        /// 应用模版文件时触发
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //获取模版里的对象
            canvas = GetTemplateChild<DrawingCanvas>("drawingCanvas");
            tbk_RightTime = GetTemplateChild<TextBlock>("tbk_RightTime");
            dragBorder = GetTemplateChild<DragContentControl>("dragBorder");

            dragBorder.OnDragBorderMove += DataChanged;
        }

        /// <summary>
        /// 绘制区域高度
        /// </summary>
        public double CanvasHeight { get => canvas.ActualHeight; }

        /// <summary>
        /// 绘制区域宽度
        /// </summary>
        public double CanvasWidth { get => canvas.ActualWidth; }

        /// <summary>
        /// 线条集合
        /// </summary>
        public List<BrokenLine> BrokenLines { get => brokenLines; }

        /// <summary>
        /// 选中区间的开始时间
        /// </summary>
        public DateTime SelectedStartTime { get => dragBorder.StartTime; set => dragBorder.StartTime = value; }

        /// <summary>
        /// 选中区间的结束时间
        /// </summary>
        public DateTime SelectedEndTime { get => dragBorder.EndTime; set => dragBorder.EndTime = value; }

        /// <summary>
        /// 选中的时间区间
        /// </summary>
        public TimeInterval SelectedTimeInterval { get => dragBorder.TimeInterval; set => dragBorder.TimeInterval = value; }


        /// <summary>
        /// 总时间区间
        /// </summary>
        public TimeInterval TimeInterval
        {
            get => timeInterval;
            set
            {
                timeInterval = value;
                SetValue(StartTimeProperty, value.StartTime);
                SetValue(EndTimeProperty, value.EndTime);
            }
        }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get => TimeInterval.StartTime;
            set
            {
                TimeInterval.StartTime = value;
                SetValue(StartTimeProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty StartTimeProperty = DependencyProperty.Register(
            "StartTime",
            typeof(DateTime),
            typeof(SHHCurveThumbnail),
            new PropertyMetadata(DateTime.Now));

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get => TimeInterval.EndTime;
            set
            {
                TimeInterval.EndTime = value;
                SetValue(EndTimeProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty EndTimeProperty = DependencyProperty.Register(
            "EndTime",
            typeof(DateTime),
            typeof(SHHCurveThumbnail),
            new PropertyMetadata(DateTime.Now));


        /// <summary>
        /// 当前区间位置
        /// </summary>
        //public Position Position { get => position; set => position = value; }
    }


}
