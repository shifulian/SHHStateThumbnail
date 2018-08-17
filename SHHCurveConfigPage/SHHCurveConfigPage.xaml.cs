using BLL;
using KJ70New.Factory;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
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

namespace SHH.CurveConfig.UI
{
    /// <summary>
    /// SHHCurveConfigPage.xaml 的交互逻辑
    /// </summary>
    public partial class SHHCurveConfigPage : Window, INotifyPropertyChanged
    {
        private DateTime startTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 00:00", DateTimeFormatInfo.InvariantInfo));
        private DateTime endTime = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd 23:59", DateTimeFormatInfo.InvariantInfo));

        //是否显示实时测点
        private bool isShowRealPoint = true;
        //是否只按端口展示曲线
        private bool isShowCurveFromPoint = false;
        //曲线名称数组
        private List<String> curveName = new List<string>();
        //传感器类型名称
        private string sensorName = String.Empty;
        //实时曲线查询条件
        private StringBuilder searchRealTime = new StringBuilder();
        //查询条件
        private StringBuilder searchWhere = new StringBuilder();
        //曲线地点列表
        private List<String> curvePlaceName=new List<string>();
        //曲线类型
        private string curveType = String.Empty;
        //时间类型
        private string timeType = String.Empty;
        //上次选中设备
        private string sensorTypeLast = string.Empty;
        //上次选中测点
        private List<string> sensorsLastList = new List<string>();


        /// <summary>
        /// 查询条件内存表
        /// </summary>
        private DataTable searchTable = new DataTable();

        //属性改变事件
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 构造函数
        /// </summary>
        public SHHCurveConfigPage()
        {
            InitializeComponent();

            DataContext = this;
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //获取内存表
            HistorySwitchManager historySwithchManager = new HistorySwitchManager();
            searchTable = historySwithchManager.GetSwitchSearch("D",isShowRealPoint);

            //更新传感器列表
            cbx_Sensor.Items.Clear();
            foreach (DataRow dataRow in searchTable.Rows)
            {
                if (!cbx_Sensor.Items.Contains(dataRow["sensorName"]))
                {
                    cbx_Sensor.Items.Add(dataRow["sensorName"]);
                }
            }
            cbx_Sensor.SelectedIndex = 0;


            //记忆上次选中的传感器类型,待实现
        }


        #region Button事件
        /// <summary>
        /// 点击按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((Button)sender is Button button)
            {
                switch (button.Name)
                {
                    case "btn_Sure":
                        Sure();
                        break;
                    case "btn_Cancel":
                        Cancel();
                        break;
                    case "btn_Restore":
                        Restore();
                        break;
                    case "btn_SaveHistory":
                        SaveHistory();
                        break;
                    case "btn_Add":
                        Add();
                        break;
                    case "btn_Delete":
                        Delete();
                        break;
                }
            }
        }

        /// <summary>
        /// 删除选中曲线
        /// </summary>
        private void Delete()
        {
            for (int i = 0; i < lst_Curve.SelectedItems.Count;)
            {
                lst_Sensor.Items.Add(this.lst_Curve.SelectedItems[i]);
                lst_Curve.Items.Remove(this.lst_Curve.SelectedItems[i]);
                if (lst_Curve.SelectedItems.Count == 0)
                {
                    ++i;
                }
                else
                {
                    i = 0;
                }
            }
        }

        /// <summary>
        /// 增加选中曲线
        /// </summary>
        private void Add()
        {
            //没有选择测点
            if (lst_Sensor.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要添加的曲线！");
                return;
            }

            //选择的曲线超过6条
            if (lst_Sensor.SelectedItems.Count + lst_Curve.Items.Count > 6)
            {
                MessageBox.Show("最多只能选择六条曲线！");
                return;
            }

            //添加曲线对象
            for (int i = 0; i <= this.lst_Sensor.SelectedItems.Count;)
            {
                lst_Curve.Items.Add(lst_Sensor.SelectedItems[i]);
                lst_Sensor.Items.Remove(lst_Sensor.SelectedItems[i]);
                if (lst_Sensor.SelectedItems.Count == 0)
                {
                    ++i;
                }
                else
                {
                    i = 0;
                }
            }
        }

        /// <summary>
        /// 还原
        /// </summary>
        private void Restore()
        {

        }

        /// <summary>
        /// 保存历史
        /// </summary>
        private void SaveHistory()
        {

        }

        /// <summary>
        /// 取消
        /// </summary>
        private void Cancel()
        {
            //窗口返回
            this.DialogResult = false;
        }



        /// <summary>
        /// 输入验证
        /// </summary>
        /// <returns></returns>
        public bool ValidateInput()
        {
            if (this.lst_Curve.Items.Count == 0)
            {
                MessageBox.Show("请添加曲线对象！");
                return false;
            }
            TimeSpan timeSpan = EndTime - startTime;
            if (timeSpan.TotalMinutes < 10)
            {
                MessageBox.Show("时间跨度最小不能小于10分钟！");
                return false;
            }

            return true;
        }
        

        /// <summary>
        /// 确定
        /// </summary>
        private void Sure()
        {
            if (ValidateInput())
            {
                for (int i = 0; i < this.lst_Curve.Items.Count; i++)
                {
                    string curve = this.lst_Curve.Items[i].ToString();
                    //点击确定，将上次数据全部归为原始状态
                    this.curveName.Clear();
                    this.SearchWhere = new StringBuilder();
                    this.SearchRealTime = new StringBuilder();
                    this.curvePlaceName.Clear();
                    this.SearchWhere.Append("(");

                    if (i != this.lst_Curve.Items.Count - 1)
                    {
                        if (!isShowCurveFromPoint)
                        {
                            this.SearchWhere.Append("(stationAddress=" + Convert.ToInt32(curve.Substring(0, 3)) + " and stationInId=" + Convert.ToInt32(curve.Substring(4, 2)) + " and simulatePlace=''" + curve.Substring(curve.IndexOf("/") + 1) + "'') or ");
                        }
                        else//只按照端口展示
                        {
                            this.SearchWhere.Append("(stationAddress=" + Convert.ToInt32(curve.Substring(0, 3)) + " and stationInId=" + Convert.ToInt32(curve.Substring(4, 2)) + ") or ");
                        }

                        this.SearchRealTime.Append("testPoint='" + Convert.ToInt32(curve.Substring(0, 3)) + "." + Convert.ToInt32(curve.Substring(4, 2)) + "' or ");

                    }
                    else
                    {
                        if (!isShowCurveFromPoint)
                        {
                            this.SearchWhere.Append("(stationAddress=" + Convert.ToInt32(curve.Substring(0, 3)) + " and stationInId=" + Convert.ToInt32(curve.Substring(4, 2)) + " and simulatePlace=''" + curve.Substring(curve.IndexOf("/") + 1) + "'')");
                        }
                        else//只按照端口展示
                        {
                            this.SearchWhere.Append("(stationAddress=" + Convert.ToInt32(curve.Substring(0, 3)) + " and stationInId=" + Convert.ToInt32(curve.Substring(4, 2)) + ")");
                        }
                        this.SearchRealTime.Append("testPoint='" + Convert.ToInt32(curve.Substring(0, 3)) + "." + Convert.ToInt32(curve.Substring(4, 2)) + "'");
                    }
                    this.curveName.Add(curve.Substring(0, 6));
                    this.curvePlaceName.Add(curve);//(curve.Substring(7));
                }
                this.SearchWhere.Append(")");
                this.SearchWhere.Append(" and sensorType=0 and sensorName=''" + cbx_Sensor.Text + "''");

                //给自定义属性赋值
                this.CurveType = "历史曲线";
                this.SensorName = cbx_Sensor.Text;

                //计算之间时间差
                TimeSpan timeSpan = endTime.Subtract(startTime);
                if (timeSpan.TotalDays > 366)
                {
                    MessageBox.Show("当前不支持一年以上数据查询");
                    return;
                }
                if (timeSpan.TotalDays > 248)
                {
                    if (this.lst_Curve.Items.Count > 2)
                    {
                        MessageBox.Show("8个月以上最好不要超过两条曲线");
                        return;
                    }
                }
            }

            //窗口返回
            this.DialogResult = true;
        }

        #endregion

        #region CheckBox事件
        /// <summary>
        /// 点击CheckBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Click(object sender, MouseButtonEventArgs e)
        {
            if ((CheckBox)sender is CheckBox checkBox)
            {
                switch (checkBox.Name)
                {
                    case "cbx_ShowCurveFromPoint":
                        ShowCurveFromPoint();
                        break;

                    case "cbx_ShowRealPoint":
                        ShowRealPoint();
                        break;
                }
            }
        }

        /// <summary>
        /// 显示实时测点
        /// </summary>
        private void ShowRealPoint()
        {

        }

        /// <summary>
        /// 只按端口展示曲线
        /// </summary>
        private void ShowCurveFromPoint()
        {

        }
        #endregion


        #region ComboBox事件
        /// <summary>
        /// 传感器类型改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cbx_Sensor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //清空ListView
            lst_Sensor.Items.Clear();
            lst_Curve.Items.Clear();

            DataRow[] lb_dataRow = searchTable.Select("sensorName='" + cbx_Sensor.SelectedItem.ToString() + "'");
            for (int i = 0; i < lb_dataRow.Length; i++)
            {
                //格式分站号
                string stationAddress = lb_dataRow[i]["stationAddress"].ToString();
                switch (stationAddress.Length)
                {
                    case 1:
                        stationAddress = "00" + stationAddress;
                        break;
                    case 2:
                        stationAddress = "0" + stationAddress;
                        break;
                }

                //格式端口号
                string stationInId = lb_dataRow[i]["stationInId"].ToString();
                if (stationInId.Length < 2)
                {
                    stationInId = "0" + stationInId;
                }

                string point = "";
                //只按照端口查询
                if (!IsShowCurveFromPoint)
                {
                    point = stationAddress + "A" + stationInId + "/" + lb_dataRow[i]["placeName"].ToString();
                    //if (!m_Sensors_Last.Contains(point) && !this.lbSensor.Items.Contains(point))
                    //{
                    //    this.lbSensor.Items.Add(point);
                    //    this.comboBox8.Items.Add(point);
                    //}

                }
                else
                {
                    point = stationAddress + "A" + stationInId;
                    //if (!m_Sensors_Last.Contains(point) && !this.lbSensor.Items.Contains(point))
                    //{
                    //    this.lbSensor.Items.Add(point);
                    //    this.comboBox8.Items.Add(point);
                    //}
                }

                this.lst_Sensor.Items.Add(point);

            }
        }
        #endregion

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get => startTime;
            set
            {
                startTime = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("StartTime"));
            }
        }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get => endTime;
            set
            {
                endTime = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("EndTime"));
            }
        }

        /// <summary>
        /// 是否显示实时测点
        /// </summary>
        public bool IsShowRealPoint
        {
            get => isShowRealPoint;
            set
            {
                isShowRealPoint = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsShowRealPoint"));
            }
        }

        /// <summary>
        /// 是否只按端口展示曲线
        /// </summary>
        public bool IsShowCurveFromPoint
        {
            get => isShowCurveFromPoint;
            set
            {
                isShowCurveFromPoint = value;
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs("IsShowCurveFromPoint"));
            }
        }

        /// <summary>
        /// 实时曲线查询条件
        /// </summary>
        public StringBuilder SearchRealTime { get => searchRealTime; set => searchRealTime = value; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public StringBuilder SearchWhere { get => searchWhere; set => searchWhere = value; }

        /// <summary>
        /// 曲线类型
        /// </summary>
        public string CurveType { get => curveType; set => curveType = value; }

        /// <summary>
        /// 传感器类型名称
        /// </summary>
        public string SensorName { get => sensorName; set => sensorName = value; }

        /// <summary>
        /// 时间类型
        /// </summary>
        public string TimeType { get => timeType; set => timeType = value; }

        /// <summary>
        /// 上次选中设备
        /// </summary>
        public string SensorTypeLast { get => sensorTypeLast; set => sensorTypeLast = value; }

        /// <summary>
        /// 上次选中测点
        /// </summary>
        public List<string> SensorsLastList { get => sensorsLastList; set => sensorsLastList = value; }
    }
}
