using Microsoft.Win32;
using SHH.CurvePage.BLL;
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
using SHH.CurveConfig.UI;
using System.ComponentModel;
using System.Data;

namespace SHH.CurvePage.UI
{
    /// <summary>
    /// SHHCurvePage.xaml 的交互逻辑
    /// </summary>
    public partial class SHHCurvePage : Window, INotifyPropertyChanged
    {
        //曲线集合
        private List<BrokenLine> brokenLines = new List<BrokenLine>();
        //是否显示最小值
        private bool isMinShow = true;
        //是否显示最大值
        private bool isMaxShow = true;
        //是否显示平均值
        private bool isAvgShow = true;

        //是否显示实时测点
        private bool isShowRealPoint = true;
        //是否只按端口展示曲线
        private bool isShowCurveFromPoint = false;
        //是否显示测点地点
        private bool isShowPointPlace = false;
        //曲线名称列表
        private List<String> curveName = new List<string>();
        //曲线地点列表
        private List<String> curvePlaceName = new List<string>();

        //实时曲线查询条件
        private String searchRealTime;
        //查询条件
        private String searchWhere;




        ///// <summary>
        ///// 查询条件(开关量)
        ///// </summary>
        //private static StringBuilder switch_search_where = new StringBuilder();

        ///// <summary>
        ///// 查询条件(断电仪)
        ///// </summary>
        //private static StringBuilder outer_search_where = new StringBuilder();

        //private StringBuilder[] conditionsArray = new StringBuilder[] { switch_search_where, outer_search_where };





        //传感器类型名称
        private string sensorName = String.Empty;
        //曲线类型
        private string curveType = String.Empty;
        //时间类型
        private string timeType = String.Empty;
        //开始时间
        private DateTime startTime = new DateTime();
        //结束时间
        private DateTime endTime = new DateTime();



        //属性改变事件
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// 构造函数
        /// </summary>
        public SHHCurvePage()
        {
            InitializeComponent();

            //设置数据相关上下文
            DataContext = this;
        }

        /// <summary>
        /// 控件载入时调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //获取参数
            String path = System.Environment.CurrentDirectory;


            //using (StreamReader reader = new StreamReader(path + @"\startTime.txt", Encoding.UTF8))
            //{
            //    startTime = Convert.ToDateTime(reader.ReadToEnd());
            //}

            using (StreamReader reader = new StreamReader(path + @"\searchWhere.txt", Encoding.UTF8))
            {
                searchWhere = reader.ReadToEnd();
            }
            using (StreamReader reader = new StreamReader(path + @"\startTime.txt", Encoding.UTF8))
            {
                startTime = Convert.ToDateTime(reader.ReadToEnd());
            }
            using (StreamReader reader = new StreamReader(path + @"\endTime.txt", Encoding.UTF8))
            {
                endTime = Convert.ToDateTime(reader.ReadToEnd());
            }
            using (StreamReader reader = new StreamReader(path + @"\sensorName.txt", Encoding.UTF8))
            {
                sensorName = reader.ReadToEnd();
            }
            using (StreamReader reader = new StreamReader(path + @"\timeType.txt", Encoding.UTF8))
            {
                timeType = reader.ReadToEnd();
            }

            try
            {
                DataSet ds = SHHCurveManager.HistorySwithchManager.GetHistorySwitchTimeBar(searchWhere, startTime, endTime, "D");

                SHHCurveManager.Dt = SHHCurveManager.HistorySwithchManager.GetHistorySwitchTimeBar(searchWhere, startTime, endTime, "D").Tables[0];
                SHHCurveManager.StartTime = startTime;
                SHHCurveManager.EndTime = endTime;
                switch (timeType)
                {
                    case "时":
                        SHHCurveManager.TimeSpan = new TimeSpan(1, 0, 0);
                        break;
                    case "分":
                        SHHCurveManager.TimeSpan = new TimeSpan(0, 1, 0);
                        break;
                    case "秒":
                        SHHCurveManager.TimeSpan = new TimeSpan(0, 0, 1);
                        break;
                    default:
                        SHHCurveManager.TimeSpan = new TimeSpan(1, 0, 0, 0);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            RePaint();
        }

        /// <summary>
        /// 重绘
        /// </summary>
        private void RePaint()
        {
            //更新数据
            brokenLines = SHHCurveManager.ReflushData();

            if (brokenLines != null)
            {
                thumbnail.BrokenLines.Clear();

                //更新缩略图曲线集合
                for (int i = 0; i < brokenLines.Count; ++i)
                {
                    thumbnail.BrokenLines.Add(brokenLines[i]);
                }

                //开始时间
                thumbnail.StartTime = SHHCurveManager.StartTime;
                //结束时间
                thumbnail.EndTime = SHHCurveManager.EndTime;
                //初始化(并触发OnDataChanged事件)
                thumbnail.Init();
            }
        }

        /// <summary>
        /// 缩略图改变时触发
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="timeInterval"></param>
        private void Thumbnail_OnDataChanged(List<BrokenLine> lines, TimeInterval timeInterval)
        {
            DateTime time = DateTime.Now;
            //重绘事件
            detail.RePaint(lines, timeInterval, 0, 1);

            //DateTime time1 = DateTime.Now;
            //MessageBox.Show("开始时间 : " + time.ToString() + "\n结束时间 : " + time1.ToString() + "\n绘制时间 : " + (time1 - time).ToString(), "标题");
        }


        // <summary>
        /// 标尺移动    (测试数据)
        /// </summary>
        /// <param name="s"></param>
        //private void Detail_OnRulerMove(double scale)
        //{
        //    listView.Items.Clear();



        //    for (int i = 0; i < brokenLines.Count; ++i)
        //    {
        //        int j = (int)(brokenLines[i].Points.Count * thumbnail.Position.p1 + (brokenLines[i].Points.Count * thumbnail.Position.p2 - brokenLines[i].Points.Count * thumbnail.Position.p1) * scale);

        //        listView.Items.Add(new { Name = brokenLines[i].Name, Time =1 });
        //    }
        //}


        /// <summary>
        /// 点击按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            String controlName = ((Button)sender).Name.ToString();

            switch (controlName)
            {
                #region 控件内部方法
                case "btn_ZoomIn":
                    detail.ZoomIn();
                    break;
                case "btn_ZoomOut":
                    detail.ZoomOut();
                    break;
                case "btn_ZoomInAxisX":
                    detail.ZoomInAxisX();
                    break;
                case "btn_ZoomOutAxisX":
                    detail.ZoomOutAxisX();
                    break;
                case "btn_ZoomInAxisY":
                    detail.ZoomInAxisY();
                    break;
                case "btn_ZoomOutAxisY":
                    detail.ZoomOutAxisY();
                    break;
                //case "++X轴标签":
                //    detail.AddAxisXLabel();
                //    break;
                //case "--X轴标签":
                //    detail.RemoveAxisXLabel();
                //    break;
                //case "++Y轴标签":
                //    detail.AddAxisYLabel();
                //    break;
                //case "--Y轴标签":
                //    detail.RemoveAxisYLabel();
                //    break;
                //case "密度增加":
                //    detail.PointDensityUp();
                //    break;
                //case "密度减少":
                //    detail.PointDensityDown();
                //    break;
                //case "显示提示":
                //    detail.ChangeToolTipShow();
                //    break;
                case "btn_ChangeCapShow":
                    detail.ChangeCapShow();
                    break;
                case "btn_ChangeRulerShow":
                    detail.ChangeRulerShow();
                    break;
                case "btn_StoreSize":
                    detail.OriginalSize();
                    break;
                #endregion

                #region 外部实现
                case "btn_FullScreen":
                    ShowFullScreen();
                    break;
                case "btn_DownLoad":
                    Download();
                    break;
                case "btn_ConfigurationCurve":
                    ConfigurationCurve();
                    break;
                case "btn_DoubleLeft":
                    DoubleMoveLeft();
                    break;
                case "btn_LeftMove":
                    LeftMove();
                    break;
                case "btn_RightMove":
                    RightMove();
                    break;
                case "btn_DoubleRight":
                    DoubleMoveRight();
                    break;
                    #endregion
            }
        }

        /// <summary>
        /// 左移到底
        /// </summary>
        private void DoubleMoveLeft()
        {
        }

        /// <summary>
        /// 左移
        /// </summary>
        private void LeftMove()
        {
            //thumbnail.MoveLeft();
        }

        /// <summary>
        /// 右移
        /// </summary>
        private void RightMove()
        {

        }

        /// <summary>
        /// 右移到底
        /// </summary>
        private void DoubleMoveRight()
        {

        }

        /// <summary>
        /// 配置曲线
        /// </summary>
        private void ConfigurationCurve()
        {
            SHHCurveConfigPage page = new SHHCurveConfigPage
            {
                Owner = this
            };

            //显示配置窗口
            if (page.ShowDialog() != null)
            {

                try
                {
                    //SHHCurveManager.Dt = SHHCurveManager.HistorySimulateManager.GetHistorySimulateByTime(page.StartTime, page.EndTime, page.SearchWhere.ToString(), "A", page.SensorName, "时", page.CurveType).Tables[0];
                    SHHCurveManager.Dt = SHHCurveManager.HistorySwithchManager.GetHistorySwitch_DayEfficiency(page.SearchWhere.ToString(), page.StartTime, page.EndTime, "D").Tables[2];
                    SHHCurveManager.StartTime = page.StartTime;
                    SHHCurveManager.EndTime = page.EndTime;
                }
                catch
                {

                }

                //曲线重绘
                RePaint();
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

        /// <summary>
        /// 是否显示最小值
        /// </summary>
        public bool IsMinShow
        {
            get => isMinShow;
            set
            {
                isMinShow = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsMinShow"));
                RePaint();
            }
        }

        /// <summary>
        /// 是否显示最大值
        /// </summary>
        public bool IsMaxShow
        {
            get => isMaxShow;
            set
            {
                isMaxShow = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("isMaxShow"));
                RePaint();
            }
        }

        /// <summary>
        /// 是否显示平均值
        /// </summary>
        public bool IsAvgShow
        {
            get => isAvgShow;
            set
            {
                isAvgShow = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("isAvgShow"));
                RePaint();
            }
        }


    }
}
