using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PreTimer
{
    /// <summary>
    /// MegTO.xaml 的交互逻辑
    /// </summary>
    public partial class MegTO : Window
    {
        public MegTO()
        {
            InitializeComponent();
            tb_meg.Text = Properties.Settings.Default.D_Entext;
        }

        private void Btu_close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
