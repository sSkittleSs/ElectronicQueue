using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ElectronicQueue.Controls
{
    class CornerTextBox : TextBox
    {
        public static readonly DependencyProperty CornerRadiusProperty;
        public static readonly DependencyProperty WatermarkForegroundProperty;
        public static readonly DependencyProperty WatermarkTextProperty;
        public static readonly DependencyProperty WatermarkVisibilityProperty;

        static CornerTextBox()
        {
            CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(CornerTextBox), new UIPropertyMetadata(new CornerRadius(2)));
            WatermarkForegroundProperty = DependencyProperty.Register("WatermarkForeground", typeof(Brush), typeof(CornerTextBox), new UIPropertyMetadata(Brushes.Gray));
            WatermarkTextProperty = DependencyProperty.Register("WatermarkText", typeof(string), typeof(CornerTextBox), new UIPropertyMetadata(""));
            WatermarkVisibilityProperty = DependencyProperty.Register("WatermarkVisibility", typeof(Visibility), typeof(CornerTextBox), new UIPropertyMetadata(Visibility.Collapsed));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(CornerTextBox), new FrameworkPropertyMetadata(typeof(CornerTextBox)));
        }

        public CornerRadius CornerRadius { get; set; }
        public Brush WatermarkForeground { get; set; }
        public string WatermarkText { get; set; }
        public Visibility WatermarkVisibility { get; set; }
    }
}
