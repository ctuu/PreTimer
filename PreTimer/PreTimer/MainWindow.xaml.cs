using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
        private MediaPlayer f_tips_mu = new MediaPlayer();
        private MediaPlayer f_over_mu = new MediaPlayer();
        private bool isTO = true;
        private bool isR = false;
        private bool crEx = false;
        private Exten at;

        public MainWindow()
        {
            InitializeComponent();

            //Application.Current.MainWindow.WindowStyle = WindowStyle.ToolWindow;
            Window w = new Window()
            {
                Top = -100, // Location of new window is outside of visible part of screen
                Left = -100,
                Width = 1, // size of window is enough small to avoid its appearance at the beginning
                Height = 1,
                WindowStyle = WindowStyle.ToolWindow, // Set window style as ToolWindow to avoid its icon in AltTab 
                ShowInTaskbar = false
            }; // Create helper window
            w.Show(); // We need to show window before set is as owner to our main window
            this.Owner = w; // Okey, this will result to disappear icon for main window.
            w.Hide(); // Hide helper window just in case
            D_Timer.Tick += new EventHandler(D_Timer_Tick);
            D_Timer.Interval = new TimeSpan(0, 0, 1);
            tb_time.Text = set_time.ToString("mm:ss");
            if (System.IO.File.Exists(Properties.Settings.Default.D_tips_mu))
                f_tips_mu.Open(new Uri(Properties.Settings.Default.D_tips_mu));
            else
            {
                Properties.Settings.Default.D_tips_mu = Environment.CurrentDirectory + @"\WAV\提示音.wav";
                Properties.Settings.Default.Save();
            }
            if (System.IO.File.Exists(Properties.Settings.Default.D_over_mu))
                f_over_mu.Open(new Uri(Properties.Settings.Default.D_over_mu));
            {
                Properties.Settings.Default.D_over_mu = Environment.CurrentDirectory + @"\WAV\终止音.wav";
                Properties.Settings.Default.Save();
            }
        }

        private void D_Timer_Tick(object sender, EventArgs e)
        {

            if (set_time == new DateTime(1, 1, 1, 0, 0, 0))
            {
                D_Timer.Stop();
                f_over_mu.Stop();
                f_over_mu.Play();
                if (Properties.Settings.Default.D_Enhance)
                {
                    Activate();
                    Window tio = new Window()
                    {
                        Topmost = true,
                        WindowState = WindowState.Normal,
                        WindowStyle = WindowStyle.None,
                        ResizeMode = ResizeMode.NoResize,
                        Height = SystemParameters.PrimaryScreenHeight,
                        Width = SystemParameters.PrimaryScreenWidth,

                        //TabIndex = 0x0effffff,
                        Top = 0,
                        Left = 0
                    };
                    DockPanel do_te = new DockPanel()
                    {
                        //Height = SystemParameters.PrimaryScreenHeight,
                        //Width = SystemParameters.PrimaryScreenWidth,

                    };
                    TextBlock tb_te = new TextBlock()
                    {
                        FontSize = 48,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        TextAlignment = TextAlignment.Center,
                        TextWrapping = TextWrapping.Wrap,
                        Text = Properties.Settings.Default.D_Entext
                    };
                    do_te.Children.Add(tb_te);
                    tio.Content = do_te;
                    tio.KeyDown += ModifyPrice_KeyDown;
                    if (System.Windows.Forms.Screen.AllScreens.Length > 1)
                    {
                        System.Windows.Forms.Screen s2 = System.Windows.Forms.Screen.AllScreens[1];
                        System.Drawing.Rectangle r2 = s2.WorkingArea;
                        tio.Top = r2.Top;
                        tio.Left = r2.Left;
                        tio.Height = r2.Height;
                        tio.Width = r2.Width;
                    }
                    tio.Show();
                    MegTO meg = new MegTO();
                    meg.ShowDialog();
                    tio.Close();
                }
                btu_Run.Content = char.ConvertFromUtf32(0xf04b);
                btu_Run.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00cc6a"));
                isTO = true;
                isR = false;
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

        private void ModifyPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)//Esc键  
            {
                ((Window)sender).Close();
            }
        }

        private void Btu_Run_Click(object sender, RoutedEventArgs e)
        {
            if (isR)
            {
                D_Timer.Stop();
                isR = false;
                btu_Run.Content = char.ConvertFromUtf32(0xf04b);
                btu_Run.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00cc6a"));
            }
            else
            {
                if (isTO)
                {
                    set_time = new DateTime(1, 1, 1, 0, Properties.Settings.Default.D_min, Properties.Settings.Default.D_sec);
                    tb_time.Text = set_time.ToString("mm:ss");
                    isTO = false;
                }
                if (!crEx)
                {
                    at = new Exten(this);
                    crEx = true;
                }
                if (System.Windows.Forms.Screen.AllScreens.Length > 1)
                {
                    System.Windows.Forms.Screen s2 = System.Windows.Forms.Screen.AllScreens[1];
                    System.Drawing.Rectangle r2 = s2.WorkingArea;
                    at.Top = r2.Top;
                    at.Left = r2.Left;
                    at.Show();
                }
                else
                {
                    at.Hide();
                }
                D_Timer.Start();
                isR = true;
                btu_Run.Content = char.ConvertFromUtf32(0xf04c);
                btu_Run.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF1081ca"));
            }
        }

        private void Btu_Stop_Click(object sender, RoutedEventArgs e)
        {
            D_Timer.Stop();
            set_time = new DateTime(1, 1, 1, 0, Properties.Settings.Default.D_min, Properties.Settings.Default.D_sec);
            tb_time.Text = set_time.ToString("mm:ss");
            isTO = true;
            isR = false;
            btu_Run.Content = char.ConvertFromUtf32(0xf04b);
            btu_Run.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00cc6a"));
        }

        private void Btu_exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
            if (System.IO.File.Exists(Properties.Settings.Default.D_tips_mu))
                f_tips_mu.Open(new Uri(Properties.Settings.Default.D_tips_mu));
            if (System.IO.File.Exists(Properties.Settings.Default.D_over_mu))
                f_over_mu.Open(new Uri(Properties.Settings.Default.D_over_mu));

        }

        private void Tb_time_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();

        }

        private void Tb_time_MouseMove(object sender, MouseEventArgs e)
        {
            Top = 0;
        }
    }
}
