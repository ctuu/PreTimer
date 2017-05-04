using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace PreTimer
{
    /// <summary>
    /// exten.xaml 的交互逻辑
    /// </summary>
    public partial class Exten : Window
    {
        public Exten(Window owner)
        {
            Owner = owner;
            InitializeComponent();
            Binding b = new Binding()
            {
                Path = new PropertyPath(TextBlock.TextProperty),
                Source = tb_exten_time,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Mode = BindingMode.OneWayToSource
            };
            MainWindow mw = Owner as MainWindow;
            mw?.tb_time.SetBinding(TextBlock.TextProperty, b);
        }
    }
}
