using SHH.UI.Curve.Core;
using SHH.UI.Curve.Pub;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SHH.UI.Curve.Detail
{
    /// <summary>
    /// 曲线图
    /// </summary>
    [TemplatePart(Name = "draw", Type = typeof(SHHCurveDetailDraw))]
    [TemplatePart(Name = "back", Type = typeof(SHHCurveDetailBackground))]
    public class SHHCurveDetail : SHHContentControl, INotifyPropertyChanged
    {
        //绘制部分
        private SHHCurveDetailDraw draw;
        //背景部分
        private SHHCurveDetailBackground back;
        //是否显示提示
        private bool isToolTipShow = true;
        //属性改变事件
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SHHCurveDetail()
        {
            LoadStyleFromAssembly("SHHStateDetail;component/Style/SHHCurveDetailStyle.xaml", UriKind.Relative, "SHHCurveDetailStyle");
        }


        /// <summary>
        /// 应用模版文件时触发
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            draw = GetTemplateChild<SHHCurveDetailDraw>("draw");
            back = GetTemplateChild<SHHCurveDetailBackground>("back");

            draw.MouseMove += DrawCanvasMouseMove;
            draw.MouseLeave += DrawCanvasMouseLeave;
        }

        /// <summary>
        /// 鼠标在绘制区移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawCanvasMouseMove(object sender, MouseEventArgs e)
        {
            //if (BrokenLines == null || BrokenLines.Count == 0)
            //{
            //    return;
            //}

            //draw.Tbk_Tip.Visibility = Visibility.Visible;
            //Point p = e.GetPosition((IInputElement)sender);
            //draw.Tbk_Tip.SetValue(Canvas.LeftProperty, p.X + 14);
            //draw.Tbk_Tip.SetValue(Canvas.TopProperty, p.Y + 12);

            ////所在位置的百分比
            //double n = p.X / draw.ActualWidth / ScaleRatioAxisX;

            ////更新tbk内容
            //draw.Tbk_Tip.Inlines.Clear();

            ////计算显示的数值
            //draw.Tbk_Tip.Inlines.Add(new Run((TimeInterval.StartTime + new TimeSpan((long)((TimeInterval.EndTime - TimeInterval.StartTime).Ticks * n))).ToString("yy/MM/dd hh:mm:ss", DateTimeFormatInfo.InvariantInfo) + "\n") { Foreground = Brushes.Gray });
            //int index = (int)(n * BrokenLines[0].Points.Count);
            //if (index < BrokenLines[0].Points.Count && index > 0)
            //{
            //    for (int i = 0; i < BrokenLines.Count; ++i)
            //    {
            //        draw.Tbk_Tip.Inlines.Add(new Run(BrokenLines[i].Name + "\t" + (BrokenLines[i].Points[index].Y * (MaxValue - MinValue) + MinValue).ToString("f2") + "\n") { Foreground = Brushes.Gray });
            //    }
            //}
        }

        /// <summary>
        /// 鼠标离开绘制区域
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawCanvasMouseLeave(object sender, MouseEventArgs e)
        {
            if (BrokenLines == null || BrokenLines.Count == 0)
            {
                return;
            }
            draw.Tbk_Tip.Visibility = Visibility.Collapsed;
        }


        /// <summary>
        /// 重绘事件
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="timeInterval"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public void RePaint(List<BrokenLine> lines, TimeInterval t, double min, double max)
        {
            if (min > max)
                throw new Exception("min > max");

            //更新数据
            MaxValue = max;
            MinValue = min;
            TimeInterval = t;
            BrokenLines = lines;

            //重绘
            RePaint();
        }

        /// <summary>
        /// 重绘
        /// </summary>
        public void RePaint()
        {
            DrawingLinesOnCanvas();
        }

        /// <summary>
        /// 调用绘制部分绘制线条,改变背景,重绘标尺
        /// </summary>
        /// <param name="lines"></param>
        private void DrawingLinesOnCanvas()
        {
            if (BrokenLines == null)
                return;


            //改变背景
            back.UpdateBackground();
            //异步绘制线条
            draw.DrawingLinesOnCanvasAsync();

            if (IsRulerShow)
            {
                //标尺时间更新
                draw.RulerMove();
            }
        }

        /// <summary>
        /// 改变标尺显示状态
        /// </summary>
        public void ChangeRulerShow()
        {
            IsRulerShow = !IsRulerShow;
        }

        /// <summary>
        /// 改变提示显示状态
        /// </summary>
        public void ChangeToolTipShow()
        {
            IsToolTipShow = !IsToolTipShow;
        }

        /// <summary>
        /// 改变线帽显示状态
        /// </summary>
        public void ChangeCapShow()
        {
            IsCapShow = !IsCapShow;
            DrawingLinesOnCanvas();
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        public RenderTargetBitmap DownloadPicture(FrameworkElement ui, int width, int height)
        {
            try
            {
                RenderTargetBitmap bmp = new RenderTargetBitmap(width, height, 96d, 96d,
                PixelFormats.Pbgra32);
                bmp.Render(ui);
                return bmp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region 缩放
        /// <summary>
        /// 原始尺寸
        /// </summary>
        public void OriginalSize()
        {
            ScaleRatioAxisX = 1;
            ScaleRatioAxisY = 1;
            DrawingLinesOnCanvas();
        }

        /// <summary>
        /// 放大
        /// </summary>
        public void ZoomIn()
        {
            ZoomInAxisX();
            ZoomInAxisY();
        }

        /// <summary>
        /// 缩小
        /// </summary>
        public void ZoomOut()
        {
            ZoomOutAxisX();
            ZoomOutAxisY();
        }

        /// <summary>
        /// X轴放大
        /// </summary>
        public void ZoomInAxisX()
        {
            ZoomAxisX(ZoomRatio);
        }

        /// <summary>
        /// X轴缩小
        /// </summary>
        public void ZoomOutAxisX()
        {
            ZoomAxisX(-ZoomRatio);
        }

        /// <summary>
        /// Y轴放大
        /// </summary>
        public void ZoomInAxisY()
        {
            ZoomAxisY(ZoomRatio);
        }

        /// <summary>
        /// Y轴缩小
        /// </summary>
        public void ZoomOutAxisY()
        {
            ZoomAxisY(-ZoomRatio);
        }

        /// <summary>
        /// 指定倍率缩放X轴
        /// </summary>
        public void ZoomAxisX(double ratio)
        {
            ScaleRatioAxisX += ratio;
            DrawingLinesOnCanvas();
        }

        /// <summary>
        /// 指定倍率缩放Y轴
        /// </summary>
        public void ZoomAxisY(double ratio)
        {
            ScaleRatioAxisY += ratio;
            DrawingLinesOnCanvas();
        }
        #endregion

        #region 标签
        /// <summary>
        /// 添加X轴标签
        /// </summary>
        public void AddAxisXLabel()
        {
            SetAxisXLabelCount(++XCount);
        }

        /// <summary>
        /// 移除X轴标签
        /// </summary>
        public void RemoveAxisXLabel()
        {
            SetAxisXLabelCount(--XCount);
        }

        /// <summary>
        /// 添加Y轴标签
        /// </summary>
        public void AddAxisYLabel()
        {
            SetAxisYLabelCount(++YCount);
        }

        /// <summary>
        /// 移除Y轴标签
        /// </summary>
        public void RemoveAxisYLabel()
        {
            SetAxisYLabelCount(--YCount);
        }


        /// <summary>
        /// 设置轴标签个数
        /// </summary>
        public void SetAxisXYLabelCount(int xCount, int yCount)
        {
            SetAxisXLabelCount(xCount);
            SetAxisXLabelCount(yCount);
        }

        /// <summary>
        /// 设置X轴标签个数
        /// </summary>
        /// <param name="xCount"></param>
        public void SetAxisXLabelCount(int xCount)
        {
            XCount = xCount;
            DrawingLinesOnCanvas();
        }

        /// <summary>
        /// 设置Y轴标签个数
        /// </summary>
        /// <param name="yCount"></param>
        public void SetAxisYLabelCount(int yCount)
        {
            YCount = yCount;
            DrawingLinesOnCanvas();
        }
        #endregion

        #region 密度
        /// <summary>
        /// 增加点的密度
        /// </summary>
        public void PointDensityUp()
        {
            int density = PointDensity * 2;
            SetPointDensity(density);
        }

        /// <summary>
        /// 减少点的密度
        /// </summary>
        public void PointDensityDown()
        {
            int density = PointDensity / 2;
            SetPointDensity(density);
        }

        /// <summary>
        /// 设置点密度
        /// </summary>
        public void SetPointDensity(int density)
        {
            PointDensity = density;
            DrawingLinesOnCanvas();
        }
        #endregion

        /// <summary>
        /// 是否显示标尺
        /// </summary>
        public bool IsRulerShow
        {
            get => draw.IsRulerShow;
            set
            {
                draw.IsRulerShow = value;
            }
        }

        /// <summary>
        /// 是否显示线帽
        /// </summary>
        public bool IsCapShow
        {
            get => draw.IsCapShow;
            set
            {
                draw.IsCapShow = value;
            }
        }

        /// <summary>
        /// 缩放倍率增量
        /// </summary>
        public double ZoomRatio
        {
            get => draw.ZoomRatio;
            set
            {
                draw.ZoomRatio = value;
            }
        }

        /// <summary>
        /// 点的密度
        /// </summary>
        public int PointDensity
        {
            get => draw.PointDensity;
            set
            {
                if (value < 1 || value > 10000)
                    return;
                draw.PointDensity = value;
            }
        }

        /// <summary>
        /// X轴缩放比例
        /// </summary>
        public double ScaleRatioAxisX
        {
            get => back.ScaleRatioAxisX;
            set
            {
                //X轴缩放比例必须大于1
                if (value < 1)
                    value = 1;
                back.ScaleRatioAxisX = value;
                draw.ScaleRatioAxisX = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ZoomPercent"));
            }
        }

        /// <summary>
        /// Y轴缩放比例
        /// </summary>
        public double ScaleRatioAxisY
        {
            get => back.ScaleRatioAxisY;
            set
            {
                if (value > 5 || value < 0.1)
                    return;
                back.ScaleRatioAxisY = value;
                draw.ScaleRatioAxisY = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ZoomPercent"));
            }
        }

        /// <summary>
        /// X轴标签个数
        /// </summary>
        public int XCount
        {
            get => back.XCount;
            set
            {
                if (value < 3 || value > 16)
                    return;
                back.XCount = value;
            }
        }

        /// <summary>
        /// Y轴标签个数
        /// </summary>
        public int YCount
        {
            get => back.YCount;
            set
            {
                if (value < 3 || value > 16)
                    return;
                back.YCount = value;
            }
        }

        /// <summary>
        /// 是否显示提示
        /// </summary>
        public bool IsToolTipShow
        {
            get => isToolTipShow;
            set
            {
                //没有变化不做操作
                if (isToolTipShow == value)
                    return;

                if (value)
                {
                    draw.MouseMove += DrawCanvasMouseMove;
                    isToolTipShow = value;
                }
                else
                {
                    draw.MouseMove -= DrawCanvasMouseMove;
                    isToolTipShow = value;
                }
            }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        public double MaxValue
        {
            get => back.MaxValue;
            set
            {
                back.MaxValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MaxValue"));
            }
        }

        /// <summary>
        /// 缩放百分比
        /// </summary>
        public int ZoomPercent
        {
            get => (int)draw.ScaleRatioAxisY;
            //get => (int)(draw.ScaleRatioAxisX * draw.ScaleRatioAxisY);
        }

        /// <summary>
        /// 最小值
        /// </summary>
        public double MinValue
        {
            get => back.MinValue;
            set
            {
                back.MinValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MinValue"));
            }
        }

        /// <summary>
        /// 曲线集合
        /// </summary>
        public List<BrokenLine> BrokenLines
        {
            get => draw.BrokenLines;
            set
            {
                draw.BrokenLines = value;
                back.BrokenLines = value;
            }
        }

        /// <summary>
        /// 时间区间
        /// </summary>
        public TimeInterval TimeInterval
        {
            get => draw.TimeInterval;
            set
            {
                draw.TimeInterval = value;
                back.TimeInterval = value;
            }
        }
    }
}
