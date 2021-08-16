using System;
using System.Collections.Generic;
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

namespace ElectronicQueue.Images.Icons
{
    /// <summary>
    /// Логика взаимодействия для Edit.xaml
    /// </summary>
    public partial class Edit : UserControl
    {
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(Edit), new UIPropertyMetadata(Brushes.Black));
        public static readonly DependencyProperty InnerBackgroundColorProperty = DependencyProperty.Register("InnerBackgroundColor", typeof(Brush), typeof(Edit), new UIPropertyMetadata(Brushes.White));
        public static readonly DependencyProperty BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(Edit), new UIPropertyMetadata(Brushes.White));

        public Brush Color { set; get; }
        public Brush InnerBackgroundColor { set; get; }
        public Brush BackgroundColor { set; get; }

        public Edit()
        {
            InitializeComponent();
        }
    }
}
