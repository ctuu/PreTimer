# PreTimer
A timer for presentation

![](https://github.com/ctuu/PreTimer/blob/master/Screenshot/PreTimer.png)

图标字体来自 [FontAwesome](https://github.com/FortAwesome/Font-Awesome/)

配置文件: [System Disk]:\Users\[username]\AppData\Local\PreTimer

### 特性：
* WPF 开发，原生高 DPI 支持
* 在扩展桌面显示副计时器
* 增强模式

### 已知Bug：
* 断开扩展模式，副窗口会保留并在下次计时开始时消失
* 移动窗口时能上下拖动

## 实现细节

+ 导入图标字体

    将字体文件加入 resource
    ``` xaml
    FontFamily="pack://application:,,,/Resources/#FontAwesome"
    ```
    使用 Unicode编码，以及 HEX 色值
    ``` cs
    btu_Run.Content = char.ConvertFromUtf32(0xf04b); 
    btu_Run.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00cc6a"));
    ```

+ 跨窗口绑定

    http://www.cnblogs.com/tcjiaan/p/4947073.html

    被绑定的对象的 XAML 加入
    ``` cs
    x:FieldModifier="public" 
    ```
    于是有
    ``` cs
    //Exten.xaml.cs
    public Exten(Window owner)
    {
        Owner = owner;
        InitializeComponent();
        Binding b = new Binding()
        {
            Path = new PropertyPath(TextBlock.TextProperty),
            Source = tb_exten_time,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, //否则 .Text 不更新
            Mode = BindingMode.OneWayToSource
        };
        MainWindow mw = Owner as MainWindow;
        mw?.tb_time.SetBinding(TextBlock.TextProperty, b);
    }
    ```

+ 程序从 Alt + Tab 中隐藏

    [Hide icon in ALT+TAB](https://social.msdn.microsoft.com/Forums/vstudio/en-US/8e3a788e-1e14-4751-a756-2d68358f898b/hide-icon-in-alttab?forum=wpf)

    ``` cs
    Window w = new Window(); // Create helper window
    w.Top = -100; // Location of new window is outside of visible part of screen
    w.Left = -100;
    w.Width = 1; // size of window is enough small to avoid its appearance at the beginning
    w.Height = 1;
    w.WindowStyle = WindowStyle.ToolWindow; // Set window style as ToolWindow to avoid its icon in AltTab 
    w.Show(); // We need to show window before set is as owner to our main window
    this.Owner = w; // Okey, this will result to disappear icon for main window.
    w.Hide(); // Hide helper window just in case
    ```

+ 代码创建新窗口并添加控件
    ``` cs
    Window tio = new Window()
    {
        Height = SystemParameters.PrimaryScreenHeight, //当前屏幕高度
        Width = SystemParameters.PrimaryScreenWidth,
        ...
    }
    DockPanel do_te = new DockPanel() {...}
    TextBlock tb_te = new TextBlock() {...}
    do_te.Children.Add(tb_te); // 将 tb_te 加入到 do_te
    tio.Content = do_te; // 将 do_te 加入到tio
    ```

+ 按键事件

    [WPF学习笔记-如何按ESC关闭窗口](http://www.cnblogs.com/theroad/p/6178633.html)
    ``` cs
    tio.KeyDown += ModifyPrice_KeyDown;

    private void ModifyPrice_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)//Esc键  
        {
            ((Window)sender).Close();
        }
    }
    ```

+ 双显示器分屏显示

    [How to show() a wpf window in secondary monitor](https://social.msdn.microsoft.com/Forums/en-US/32d60663-8264-4153-9fb0-7053468191f2/how-to-show-a-wpf-window-in-secondary-monitor?forum=wpf)

    ``` cs
    //App.xaml.cs
    using System.Drawing; //需先添加引用
    using System.Windows.Forms; //需先添加引用

    // MainWindow.xaml.cs
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
    meg.ShowDialog(); //在主显示器显示，阻塞代码
    tio.Close();
    ```

+ 计时器
    ``` cs
    DispatcherTimer D_Timer = new DispatcherTimer();
    D_Timer.Tick += new EventHandler(D_Timer_Tick);
    D_Timer.Interval = new TimeSpan(0, 0, 1);

    private void D_Timer_Tick(object sender, EventArgs e) {...}
    ```

+ TabControl 与 TabItem 自定义

    [自定义 WPF TabControl 的样式/模板](http://www.cnblogs.com/wpf_gd/articles/1707750.html)
    ``` xaml
    //setting.xaml
    <Style x:Key="tabCon_style" TargetType="{x:Type TabControl}">
        ...
            <DockPanel x:Name="HeaderPanel" MouseLeftButtonDown="Tab_Dock_1_MouseLeftButtonDown" ...
            //将原来的 TabPanel 改成 DockPanel 并加入 MouseLeftButtonDown 触发 DragMove() 效果
        ...
    </Style>
    ```

    于是 TabItem 可以用
    ``` xaml
    DockPanel.Dock="..."
    ```
    调整位置

    ``` xaml
    <TabItem>
        <TabItem.Header >
            <Button .../> //将 Button 放进 TabItem.Header 实现关闭按钮
        </TabItem.Header>
    </TabItem>
    ```

+ Hyberlink 的使用
    ``` xaml
    <Hyperlink NavigateUri="mailto:username@domainname" Click="Hyperlink_Click">word</Hyperlink>// 邮箱
    ```
    ``` cs
    private void Hyperlink_Click(object sender, RoutedEventArgs e)
    {
        System.Windows.Documents.Hyperlink link = sender as System.Windows.Documents.Hyperlink;
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(link.NavigateUri.AbsoluteUri));
    }
    ```

+ 只能输入限定位数的数字
    ``` cs
    //Setting.xaml.cs
    private void Tb_Min_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        try
        {
            int val = Convert.ToInt32(tb_Min.Text);
            if (val > 59)
                tb_Min.Text = "";
            if (tb_Min.Text.Length > 2)
                tb_Min.Text = tb_Min.Text.Remove(tb_Min.Text.Length - 1, 1);
        }
        catch
        {
            tb_Min.Text = "";
        }
    }
    ```

+ 打开文件
    ``` cs
    // Setting.xaml.cs
    Microsoft.Win32.OpenFileDialog over_mu = new Microsoft.Win32.OpenFileDialog()
    {
        InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
        DefaultExt = "*.mp3;*.wav",
        Filter = "常用文件|*.mp3;*.wav|所有文件 (*.*)|*.*"
    };
    ```
    
+ 杂项
    ``` cs
    string word = Environment.CurrentDirectory (+ @"\...\file.*"); // 获取当前目录
    Application.Current.Shutdown(); // 关闭整个程序
    ```
