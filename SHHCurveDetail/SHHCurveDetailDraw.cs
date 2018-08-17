using SHH.CurvePage.Model;
using SHH.UI.Curve.Core;
using SHH.UI.Curve.Pub;
using SHH.UI.Curve.Thumbnail;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SHH.UI.Curve.Detail
{
    /// <summary>
    /// 曲线图绘制区域
    /// </summary>
    [TemplatePart(Name = "ruler", Type = typeof(RulerContentControl))]
    [TemplatePart(Name = "canvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "tbk_Tip", Type = typeof(TextBlock))]
    public class SHHCurveDetailDraw : SHHContentControl, INotifyPropertyChanged
    {
        //外层面板
        private Canvas canvas;
        //线条集合
        private List<BrokenLine> brokenLines = new List<BrokenLine>();
        //标尺
        private RulerContentControl ruler;
        //Y轴比例
        private double scaleY = 1;
        //X轴比例
        private double scaleX = 1;
        //是否显示线帽
        private bool isCapShow = true;
        //密度,默认256
        private int pointDensity = 256;
        //鼠标悬停时显示的值标签
        private TextBlock tbk_Tip;
        //缩放倍率增量,默认0.2
        private double zoomRatio = 0.2;
        //时间区间
        private TimeInterval timeInterval = new TimeInterval();
        //属性改变事件
        public event PropertyChangedEventHandler PropertyChanged;
        //绘制面板父容器
        public ContentControl drawingCanvasHost;
        ////可视化容器
        //private HostVisual hostVisual = new HostVisual();
        //锁
        private readonly object obj = new object();
        ////绘制线程
        //private Thread drawingThread;
        private DrawingCanvas drawingCanvas;
        private BrushCollection Brush = new BrushCollection();



        /// <summary>
        /// 构造函数
        /// </summary>
        public SHHCurveDetailDraw()
        {
            LoadStyleFromAssembly("SHHStateDetail;component/Style/SHHCurveDetailDrawStyle.xaml", UriKind.Relative, "SHHCurveDetailDrawStyle");
            DataContext = this;
        }

        #region 绘制代码
        /// <summary>
        /// 异步绘制线条
        /// </summary>
        public void DrawingLinesOnCanvasAsync()
        {
            //lock (obj)
            //{
            //    HostVisual hostVisual = new HostVisual();
            //    Thread drawingThread = CreateNewThreadContainer(hostVisual, drawingCanvasHost, new ParameterizedThreadStart((PreDrawingLinesOnCanvas)));
            //    drawingThread.Start(hostVisual);
            //}
            DrawingLinesOnCanvas(drawingCanvas);
        }

        /// <summary>
        /// 绘制前预处理
        /// </summary>
        /// <param name="hostVisual"></param>
        [Obsolete("已弃用", false)]
        public void PreDrawingLinesOnCanvas(object hostVisual)
        {
            VisualTarget visualTarget = new VisualTarget((HostVisual)hostVisual);
            DrawingCanvas drawingCanvas = new DrawingCanvas();
            drawingCanvas.Arrange(new Rect(0, 0, CanvasWidth, CanvasHeight));
            visualTarget.RootVisual = drawingCanvas;
            DrawingLinesOnCanvas(drawingCanvas);
            System.Windows.Threading.Dispatcher.Run();
        }

        /// <summary>
        /// 在画板上绘制折线
        /// </summary>
        private void DrawingLinesOnCanvas(DrawingCanvas drawingCanvas)
        {
            drawingCanvas.ClearVisual();

            List<BrokenLine> newLines = new List<BrokenLine>();
            foreach (var line in BrokenLines)
            {
                newLines.Add(line.DeepClone());
            }

            //获取拉伸过的曲线
            GetStretchLines(newLines);
            //遍历曲线
            for (int i = 0; i < newLines.Count; ++i)
            {
                //每条线作为一个visual对象
                DrawingVisual visual = new DrawingVisual();
                DrawingLinesOnVisual(visual, newLines[i], i);
                drawingCanvas.AddVisual(visual);
            }
        }

        /// <summary>
        /// 在Visual中绘制折线
        /// </summary>
        /// <param name="visual"></param>
        /// <param name="brokenLine"></param>
        /// <param name="index">组数</param>
        private void DrawingLinesOnVisual(DrawingVisual visual, BrokenLine brokenLine, int index)
        {
            using (DrawingContext dc = visual.RenderOpen())
            {
                int pointCount = (int)(brokenLine.Points.Count * (1 - (ScaleRatioAxisX - 1) / ScaleRatioAxisX));

                for (int i = 1; i < pointCount; ++i)
                {
                    DrawingLine(dc,
                        brokenLine.Thickness,
                        brokenLine.Points[i - 1],
                        brokenLine.Points[i],
                        index + 1
                        );
                }


            }
        }

        /// <summary>
        /// 获取拉伸过的曲线
        /// </summary>
        /// <returns></returns>
        private List<BrokenLine> GetStretchLines(List<BrokenLine> lines)
        {
            if (lines.Count == 0)
                return lines;

            //每n个绘制一次
            int n = lines[0].Points.Count / pointDensity + 1;
            //一共nCount个点
            //int nCount = lines[0].Points.Count / n;

            //转换X轴比例
            for (int i = 0; i < lines.Count; ++i)
            {
                List<StatePoint> points = new List<StatePoint>();
                //for (int j = 0; j < Math.Round((lines[i].Points.Count / ScaleRatioAxisX), MidpointRounding.AwayFromZero); j += n)
                for (int j = 0; j < (lines[i].Points.Count / ScaleRatioAxisX); j += n)
                {
                    StatePoint p = lines[i].Points[j];
                    points.Add(p);
                }
                lines[i].Points = points;
            }

            #region

            ////每n个绘制一次
            //int n = lines[0].Points.Count / pointDensity + 1;
            ////一共nCount个点
            //int nCount = lines[0].Points.Count / n;

            ////转换X轴比例
            //for (int i = 0; i < lines.Count; ++i)
            //{
            //    List<Point> points = new List<Point>();
            //    //for (int j = 0; j < lines[i].Points.Count; j += n)
            //    //{
            //    //Point p = new Point(1.0 / (nCount - 1) * j / n,
            //    //lines[i].Points[j].Y);
            //    //if (p.X > 1)
            //    //break;
            //    //points.Add(p);
            //    //}
            //    //lines[i].Points = points;
            //}
            #endregion

            return lines;
        }

        #region  绘制线帽
        /// <summary>
        /// 绘制线帽
        /// </summary>
        private void DrawLineCap(DrawingContext dc, Brush brush, Point p, BrokenLineCapType type)
        {
            switch (type)
            {
                case BrokenLineCapType.None:
                    break;

                case BrokenLineCapType.Circle:
                    dc.DrawEllipse(brush, new Pen(brush, 1), new Point(p.X * canvas.ActualWidth * ScaleRatioAxisX, canvas.ActualHeight - p.Y * canvas.ActualHeight / ScaleRatioAxisY), 1.5, 1.5);
                    break;

                case BrokenLineCapType.RectangleH:
                    dc.DrawRectangle(brush, new Pen(brush, 1), new Rect(p.X * canvas.ActualWidth * ScaleRatioAxisX - 2, canvas.ActualHeight - (p.Y * canvas.ActualHeight) / ScaleRatioAxisY - 1, 4, 2));
                    break;
                case BrokenLineCapType.RectangleV:
                    dc.DrawRectangle(brush, new Pen(brush, 1), new Rect(p.X * canvas.ActualWidth * ScaleRatioAxisX - 1, canvas.ActualHeight - (p.Y * canvas.ActualHeight) / ScaleRatioAxisY - 2, 2, 4));
                    break;

                case BrokenLineCapType.Square:
                    dc.DrawRectangle(brush, new Pen(brush, 1), new Rect(p.X * canvas.ActualWidth * ScaleRatioAxisX - 1.5, canvas.ActualHeight - (p.Y * canvas.ActualHeight) / ScaleRatioAxisY - 1.5, 3, 3));
                    break;

                case BrokenLineCapType.Triangle:
                    dc.DrawGeometry(brush, new Pen(brush, 1),
                        new PathGeometry()
                        {
                            Figures = new PathFigureCollection()
                            {
                                new PathFigure()
                                {
                                    Segments = new PathSegmentCollection
                                    {
                                        new PolyLineSegment
                                        {
                                            Points = new PointCollection(new Point[2]{  new Point(p.X * canvas.ActualWidth * ScaleRatioAxisX + 1.5, canvas.ActualHeight- (p.Y * canvas.ActualHeight) / ScaleRatioAxisY-1.5), new Point(p.X * canvas.ActualWidth * ScaleRatioAxisX, canvas.ActualHeight- (p.Y * canvas.ActualHeight) / ScaleRatioAxisY+2) })
                                        },
                                    },
                                    IsFilled = true,
                                    IsClosed = true,
                                    StartPoint = new Point(p.X * canvas.ActualWidth* ScaleRatioAxisX - 1.5,canvas.ActualHeight- (p.Y * canvas.ActualHeight )/ScaleRatioAxisY-1.5)
                                }
                            }
                        });
                    break;
                case BrokenLineCapType.TriangleR:
                    dc.DrawGeometry(brush, new Pen(brush, 1),
                        new PathGeometry()
                        {
                            Figures = new PathFigureCollection()
                            {
                                new PathFigure()
                                {
                                    Segments = new PathSegmentCollection
                                    {
                                        new PolyLineSegment
                                        {
                                            Points = new PointCollection(new Point[2]{  new Point(p.X * canvas.ActualWidth * ScaleRatioAxisX + 1.5, canvas.ActualHeight- (p.Y * canvas.ActualHeight ) / ScaleRatioAxisY+1.5), new Point(p.X * canvas.ActualWidth * ScaleRatioAxisX, canvas.ActualHeight - (p.Y * canvas.ActualHeight) / ScaleRatioAxisY-2) })
                                        },
                                    },
                                    IsFilled = true,
                                    IsClosed = true,
                                    StartPoint = new Point(p.X * canvas.ActualWidth* ScaleRatioAxisX - 1.5,canvas.ActualHeight- (p.Y * canvas.ActualHeight)/ScaleRatioAxisY+1.5)
                                }
                            }
                        });
                    break;
                case BrokenLineCapType.Diamond:
                    dc.DrawGeometry(brush, new Pen(brush, 1),
                        new PathGeometry()
                        {
                            Figures = new PathFigureCollection()
                            {
                                    new PathFigure()
                                    {
                                        Segments=new PathSegmentCollection
                                        {
                                            new PolyLineSegment
                                            {
                                                Points=new PointCollection(new Point[3]{ new Point(p.X * canvas.ActualWidth* ScaleRatioAxisX, canvas.ActualHeight - (p.Y * canvas.ActualHeight) / ScaleRatioAxisY-1.7),
                                                    new Point(p.X * canvas.ActualWidth* ScaleRatioAxisX+1.7, canvas.ActualHeight - (p.Y * canvas.ActualHeight) / ScaleRatioAxisY),
                                                    new Point(p.X * canvas.ActualWidth* ScaleRatioAxisX, canvas.ActualHeight - (p.Y * canvas.ActualHeight) / ScaleRatioAxisY+1.7)})
                                            },
                                        },
                                        IsFilled=true,
                                        IsClosed=true,
                                        StartPoint=new Point(p.X * canvas.ActualWidth* ScaleRatioAxisX - 1.7,canvas.ActualHeight- (p.Y * canvas.ActualHeight)/ScaleRatioAxisY)
                                    }
                            }
                        });
                    break;
            }
        }
        #endregion  

        /// <summary>
        /// 绘制线条
        /// </summary>
        /// <param name="dc"></param>
        /// <param name="brush"></param>
        /// <param name="thickness"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="index">组数</param>
        private void DrawingLine(DrawingContext dc, double thickness, StatePoint p1, StatePoint p2, int index)
        {
            //Point结构里存的是位置的比例,这里要做一下转换
            ////Canvas的坐标原点在左上角
            Point point1 = new Point(p1.point.X * canvas.ActualWidth, canvas.ActualHeight - canvas.ActualHeight * index / (brokenLines.Count + 1.0));
            Point point2 = new Point(p2.point.X * canvas.ActualWidth, canvas.ActualHeight - (canvas.ActualHeight * index / (brokenLines.Count + 1.0) - thickness));
            dc.DrawRectangle(Brush[p1.state], new Pen(), new Rect(point1, point2));
        }
        #endregion

        /// <summary>
        /// 应用模版文件时触发
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //获取模版里的对象
            canvas = GetTemplateChild<Canvas>("canvas");
            ruler = GetTemplateChild<RulerContentControl>("ruler");
            tbk_Tip = GetTemplateChild<TextBlock>("tbk_Tip");
            drawingCanvasHost = GetTemplateChild<ContentControl>("drawingCanvasHost");
            drawingCanvas = GetTemplateChild<DrawingCanvas>("drawingCanvas");

            //添加标尺移动事件
            ruler.OnRulerMove += RulerMove;
        }

        /// <summary>
        /// 标尺移动时触发
        /// </summary>
        /// <param name="scale"></param>
        public void RulerMove(double s)
        {
            if (Double.IsNaN(s) || s > 1)
                return;
            ruler.Time = StartTime + new TimeSpan((long)((EndTime - StartTime).Ticks * s));
        }

        /// <summary>
        /// 标尺移动时触发
        /// </summary>
        /// <param name="scale"></param>
        public void RulerMove()
        {
            if (Double.IsNaN(scaleX) || scaleX > 1)
                return;
            ruler.Time = StartTime + new TimeSpan((long)((EndTime - StartTime).Ticks * scaleX));
        }

        /// <summary>
        /// 绘制面板高度
        /// </summary>
        public double CanvasHeight { get => canvas.ActualHeight; }

        /// <summary>
        /// 绘制面板宽度
        /// </summary>
        public double CanvasWidth { get => canvas.ActualWidth; }

        /// <summary>
        /// 线条集合
        /// </summary>
        public List<BrokenLine> BrokenLines { get => brokenLines; set => brokenLines = value; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get => TimeInterval.StartTime; set => TimeInterval.StartTime = value; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get => TimeInterval.EndTime; set => TimeInterval.EndTime = value; }

        /// <summary>
        /// 标尺是否显示
        /// </summary>
        public bool IsRulerShow
        {
            get
            {
                if (ruler.Visibility == Visibility.Visible)
                    return true;
                return false;
            }
            set
            {
                if (value)
                    ruler.Visibility = Visibility.Visible;
                else
                    ruler.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 是否显示线帽
        /// </summary>
        public bool IsCapShow
        {
            get => isCapShow;
            set => isCapShow = value;
        }

        /// <summary>
        /// 缩放倍率增量
        /// </summary>
        public double ZoomRatio
        {
            get => zoomRatio;
            set
            {
                zoomRatio = value;
            }
        }

        /// <summary>
        /// 密度
        /// </summary>
        public int PointDensity
        {
            get => pointDensity;
            set
            {
                if (pointDensity < 1 || pointDensity > 10000)
                    return;
                pointDensity = value;
            }
        }

        /// <summary>
        /// 鼠标悬停时显示的值标签
        /// </summary>
        public TextBlock Tbk_Tip { get => tbk_Tip; }



        /// <summary>
        /// Y轴缩放比例
        /// </summary>
        public double ScaleRatioAxisY { get => scaleY; set => scaleY = value; }

        /// <summary>
        /// X轴缩放比例
        /// </summary>
        public double ScaleRatioAxisX { get => scaleX; set => scaleX = value; }

        /// <summary>
        /// 时间区间
        /// </summary>
        public TimeInterval TimeInterval { get => timeInterval; set => timeInterval = value; }
    }
}
