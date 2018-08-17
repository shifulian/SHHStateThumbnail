using Microsoft.Win32;
using SHH.UI.Curve.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //每条曲线点的个数
            int pNum = 5000;

            //曲线1
            List<StatePoint> points1 = new List<StatePoint>();
            int p = new Random().Next(0, pNum / 2);
            points1.Add(new StatePoint() { state = 0, point = new Point(0, 0) });
            int i = 1;
            for (; p < pNum; ++i)
            {
                //Y轴坐标暂时不计算
                points1.Add(new StatePoint() { state = i, point = new Point(p / (pNum + 0.0), 0) });

                p += new Random().Next(0, pNum / 2);
            }
            points1.Add(new StatePoint() { state = i, point = new Point(1, 0) });
            BrokenLine line1 = new BrokenLine(points1, 10, "状态1");
           
            thumbnail.BrokenLines.Add(line1);
            thumbnail.BrokenLines.Add(line1);

            //开始时间
            thumbnail.StartTime = new DateTime(2017, 1, 1);
            //结束时间
            thumbnail.EndTime = new DateTime(2018, 1, 1);
            //初始化
            thumbnail.Init();
        }

        /// <summary>
        /// 缩略图改变时触发
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="timeInterval"></param>
        private void Thumbnail_OnDataChanged(List<BrokenLine> lines, TimeInterval timeInterval)
        {
            //添加中间层来获取最大最小值

            //重绘事件
            detail.RePaint(lines, timeInterval, 0, 0.3);
        }

        /// <summary>
        /// 点击按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string s = ((Button)sender).Content.ToString();
            switch (s)
            {
                case "放大":
                    detail.ZoomIn();
                    break;
                case "缩小":
                    detail.ZoomOut();
                    break;
                case "X轴放大":
                    detail.ZoomInAxisX();
                    break;
                case "X轴缩小":
                    detail.ZoomOutAxisX();
                    break;
                case "Y轴放大":
                    detail.ZoomInAxisY();
                    break;
                case "Y轴缩小":
                    detail.ZoomOutAxisY();
                    break;
                case "++X轴标签":
                    detail.AddAxisXLabel();
                    break;
                case "--X轴标签":
                    detail.RemoveAxisXLabel();
                    break;
                case "++Y轴标签":
                    detail.AddAxisYLabel();
                    break;
                case "--Y轴标签": 
                    detail.RemoveAxisYLabel();
                    break;
                case "密度增加":
                    detail.PointDensityUp();
                    break;
                case "密度减少":
                    detail.PointDensityDown();
                    break;
                case "启用圆点":
                    detail.ChangeCapShow();
                    break;
                case "显示标尺":
                    detail.ChangeRulerShow();
                    break;
                case "显示提示":
                    detail.ChangeToolTipShow();
                    break;
                case "原始尺寸":
                    detail.OriginalSize();
                    break;

                #region 外部实现
                case "全屏显示":
                    ShowFullScreen();
                    break;
                case "下载":
                    Download();
                    break;
                    #endregion
            }
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        private void Download()
        {
            RenderTargetBitmap bmp = detail.DownloadPicture(detail, (int)detail.ActualWidth, (int)detail.ActualHeight);

            SaveFileDialog dialog = new SaveFileDialog
            {
                Title = "保存到图片",
                Filter = "图像文件(*.png)|*.png|所有文件(*.*)|*.*"
            };
            bool? b = dialog.ShowDialog();
            if (b == null || b == false)
            {
                return;
            }

            try
            {
                using (FileStream fs = new FileStream(dialog.FileName, FileMode.Create))
                {
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(bmp));
                    encoder.Save(fs);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 全屏
        /// </summary>
        private void ShowFullScreen()
        {
            this.WindowState = WindowState.Maximized;
            detail.RePaint();
        }
    }
}