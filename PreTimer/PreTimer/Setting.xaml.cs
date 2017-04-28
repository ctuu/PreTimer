using System;
using System.Windows;

namespace PreTimer
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        private System.Windows.Media.MediaPlayer f_tips_mu = new System.Windows.Media.MediaPlayer();
        private System.Windows.Media.MediaPlayer f_over_mu = new System.Windows.Media.MediaPlayer();
        public Setting()
        {
            InitializeComponent();
            tb_Min.Text = Convert.ToString(Properties.Settings.Default.D_min);
            tb_Sec.Text = Convert.ToString(Properties.Settings.Default.D_sec);
            tb_tips_mu.Text = Properties.Settings.Default.D_tips_mu;
            tb_over_mu.Text = Properties.Settings.Default.D_over_mu;
        }

        private void Btu_Setting_save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.D_min = Convert.ToInt32(tb_Min.Text);
            Properties.Settings.Default.D_sec = Convert.ToInt32(tb_Sec.Text);
            Properties.Settings.Default.D_tips_min = Convert.ToInt32(tb_tips_Min.Text);
            Properties.Settings.Default.D_tips_sec = Convert.ToInt32(tb_tips_Sec.Text);
            Properties.Settings.Default.Save();
        }
        private void Btu_tips_mu_load_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog tips_mu = new Microsoft.Win32.OpenFileDialog()
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                DefaultExt = "*.mp3;*.wav",
                Filter = "*|.mp3;*.wav"
            };
            if (tips_mu.ShowDialog() == true)
            {
                tb_tips_mu.Text = tips_mu.FileName;
                Properties.Settings.Default.D_tips_mu = tips_mu.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void Btu_tips_mu_test_Click(object sender, RoutedEventArgs e)
        {
            if (!System.IO.File.Exists(Properties.Settings.Default.D_tips_mu))
            {
                MessageBox.Show("File not exist.");
            }
            else
            {
                f_tips_mu.Stop();
                f_tips_mu.Play();
            }
        }

        private void Btu_over_mu_load_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog over_mu = new Microsoft.Win32.OpenFileDialog()
            {
                InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                DefaultExt = "*.mp3;*.wav",
                Filter = "*|.mp3;*.wav"
            };
            if (over_mu.ShowDialog() == true)
            {
                tb_over_mu.Text = over_mu.FileName;
                Properties.Settings.Default.D_over_mu = over_mu.FileName;
                Properties.Settings.Default.Save();
            }
        }

        private void Btu_over_mu_test_Click(object sender, RoutedEventArgs e)
        {
            if (!System.IO.File.Exists(Properties.Settings.Default.D_over_mu))
            {
                MessageBox.Show("File not exist.");
            }
            else
            {
                f_over_mu.Stop();
                f_over_mu.Play();
            }
        }

        private void Btu_about_Click(object sender, RoutedEventArgs e)
        {
            About a = new About();
            a.ShowDialog();
        }
    }

}
