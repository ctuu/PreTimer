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
        private bool isTO = true;
        private bool isR = false;
        
        public MainWindow()
        {
            InitializeComponent();
            D_Timer.Tick += new EventHandler(d_Timer_Tick);
            D_Timer.Interval = new TimeSpan(0, 0, 1);
            tb_time.Text = set_time.ToString("mm:ss");
            tb_Min.Text = Convert.ToString(Properties.Settings.Default.D_min);
            tb_Sec.Text = Convert.ToString(Properties.Settings.Default.D_sec);
            tb_tips_mu.Text = Properties.Settings.Default.D_tips_mu;
        }

        private void btu_Setting_save_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.D_min = Convert.ToInt32(tb_Min.Text);
            Properties.Settings.Default.D_sec = Convert.ToInt32(tb_Sec.Text);
            Properties.Settings.Default.Save();
            if (!isR && isTO)
            {
                set_time = new DateTime(1, 1, 1, 0, Properties.Settings.Default.D_min, Properties.Settings.Default.D_sec);
                tb_time.Text = set_time.ToString("mm:ss");
            }
        }
        private void d_Timer_Tick(object sender, EventArgs e)
        {
            if (set_time == new DateTime(1,1,1,0,0,0))
            {
                D_Timer.Stop();
                tb_time.Text = "Time over";
                btu_Run.Content = "Run";
                isTO = true;
            }
            else
            {
                set_time = set_time.AddSeconds(-1);
                tb_time.Text = set_time.ToString("mm:ss");
            }
        }
        private void btu_Run_Click(object sender, RoutedEventArgs e)
        {
            if (isR)
            {
                D_Timer.Stop();
                isR = false;
                btu_Run.Content = "Run";
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
                btu_Run.Content = "Pause";
            }
        }

        private void btu_Stop_Click(object sender, RoutedEventArgs e)
        {
            D_Timer.Stop();
            set_time = new DateTime(1, 1, 1, 0, Properties.Settings.Default.D_min, Properties.Settings.Default.D_sec);
            tb_time.Text = set_time.ToString("mm:ss");
            isTO = true;
            isR = false;
            btu_Run.Content = "Run";
        }

        private void btu_tips_mu_load_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog tips_mu = new Microsoft.Win32.OpenFileDialog();
            tips_mu.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            tips_mu.DefaultExt = "*.mp3;*.wav";
            tips_mu.Filter = "*|.mp3;*.wav";
            if (tips_mu.ShowDialog() == true)
            {
                tb_tips_mu.Text = tips_mu.FileName;
                Properties.Settings.Default.D_tips_mu = tips_mu.FileName;
                Properties.Settings.Default.Save();
            }

        }

        private void btu_tips_mu_test_Click(object sender, RoutedEventArgs e)
        {
            if (!System.IO.File.Exists(Properties.Settings.Default.D_tips_mu))
            {
                MessageBox.Show("File not exist.");
            }
            else
            {
                
                f_tips_mu.Open(new Uri(Properties.Settings.Default.D_tips_mu));
                f_tips_mu.Stop();
                //f_tips_mu.Position = new TimeSpan(0, 0, 0);
                f_tips_mu.Play();
            }
        }
    }
}
