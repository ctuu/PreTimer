using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Drawing;
using System.Windows.Forms;

namespace PreTimer
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            MainWindow w1 = new MainWindow();
            //Exten w2 = new Exten();
            Screen s1 = Screen.AllScreens[0];
            //Rectangle r1 = s1.WorkingArea;
            //w1.Top = r1.Top;
            //w1.Left = r1.Left;
            w1.Show();
            if (Screen.AllScreens.Count() > 1)
            {
                Screen s2 = Screen.AllScreens[1];
                Rectangle r2 = s2.WorkingArea;
                //w2.Top = r2.Top;
                //w2.Left = r2.Left;
                //w2.Show();
                //w2.Owner = w1;
            }
        }
    }
}
