using System;
using System.Windows;
using System.Windows.Threading;

namespace PreTimer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DateTime set_time = new DateTime(1, 1, 1, 0, Properties.Settings.Default.D_min, Properties.Settings.Default.D_sec);
        private DispatcherTimer D_Timer = new DispatcherTimer();
        private System.Windows.Media.MediaPlayer f_tips_mu = new System.Windows.Media.MediaPlayer();
        private System.Windows.Media.MediaPlayer f_over_mu = new System.Windows.Media.MediaPlayer();
        private bool isTO = true;
        private bool isR = false;
        
        public MainWindow()
        {
            InitializeComponent();
            D_Timer.Tick += new EventHandler(D_Timer_Tick);
            D_Timer.Interval = new TimeSpan(0, 0, 1);
            tb_time.Text = set_time.ToString("mm:ss");
            f_tips_mu.Open(new Uri(Properties.Settings.Default.D_tips_mu));
            f_over_mu.Open(new Uri(Properties.Settings.Default.D_over_mu));
        }

        
        private void D_Timer_Tick(object sender, EventArgs e)
        {

            if (set_time == new DateTime(1,1,1,0,0,0))
            {
                D_Timer.Stop();
                f_over_mu.Stop();
                f_over_mu.Play();
                tb_time.Text = "Time over";
                btu_Run.Content = char.ConvertFromUtf32(0xE102);
                isTO = true;
            }
            else
            {
                if (set_time == new DateTime(1, 1, 1, 0, Properties.Settings.Default.D_tips_min, Properties.Settings.Default.D_tips_sec))
                {
                    f_tips_mu.Stop();
                    f_tips_mu.Play();
                }
                set_time = set_time.AddSeconds(-1);
                tb_time.Text = set_time.ToString("mm:ss");
            }
        }
        private void Btu_Run_Click(object sender, RoutedEventArgs e)
        {
            if (isR)
            {
                D_Timer.Stop();
                isR = false;
                btu_Run.Content = char.ConvertFromUtf32(0xE102);
            }
            else
            {
                if (isTO)
                {
                    set_time = new DateTime(1, 1, 1, 0, Properties.Settings.Default.D_min, Properties.Settings.Default.D_sec);
                    tb_time.Text = set_time.ToString("mm:ss");
                    isTO = false;
                }
                D_Timer.Start();
                isR = true;
                btu_Run.Content = char.ConvertFromUtf32(0xE103);
            }
        }

        private void Btu_Stop_Click(object sender, RoutedEventArgs e)
        {
            D_Timer.Stop();
            set_time = new DateTime(1, 1, 1, 0, Properties.Settings.Default.D_min, Properties.Settings.Default.D_sec);
            tb_time.Text = set_time.ToString("mm:ss");
            isTO = true;
            isR = false;
            btu_Run.Content = char.ConvertFromUtf32(0xE102);
        }

        private void btu_exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Btu_setting_Click(object sender, RoutedEventArgs e)
        {
            f_tips_mu.Close();
            f_over_mu.Close();
            Setting a = new Setting();
            a.ShowDialog();
            if (!isR && isTO)
            {
                set_time = new DateTime(1, 1, 1, 0, Properties.Settings.Default.D_min, Properties.Settings.Default.D_sec);
                tb_time.Text = set_time.ToString("mm:ss");
            }
            f_tips_mu.Open(new Uri(Properties.Settings.Default.D_tips_mu));
            f_over_mu.Open(new Uri(Properties.Settings.Default.D_over_mu));

        }
    }
}
