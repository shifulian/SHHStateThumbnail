using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using SHH.CurvePage.BLL;
using SHH.CurvePage.UI;

namespace SHH.CurvePage
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        [STAThread]
        static void Main(String[] args)
        {
            App app = new App
            {
                MainWindow = new SHHCurvePage()
            };
            app.MainWindow.ShowDialog();


            //App app = new App();
            //SHHCurvePage sHHCurvePage = new SHHCurvePage();
            //app.Run(sHHCurvePage);

            //App app = new App();
            //SHHCurvePage sHHCurvePage = new SHHCurvePage();
            //app.MainWindow=sHHCurvePage;
            //SHHCurvePage.Show();
            //app.Run();

        }
    }


    //public partial class Startup : Application
    //{
    //    [STAThread]
    //    static void Main()
    //    {
    //        Startup app = new Startup();
    //        app.InitializeComponent();
    //        app.Run();

    //    }

    //    public void InitializeComponent()
    //    {
    //        this.StartupUri = new Uri("MainWindow.xaml", System.UriKind.Relative);
    //    }

    //}
}
