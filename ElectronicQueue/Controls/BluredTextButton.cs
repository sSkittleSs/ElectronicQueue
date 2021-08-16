using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ElectronicQueue.Controls
{
    class BluredTextButton : Button
    {
        public static readonly DependencyProperty BluredForegroundProperty;
        public static readonly DependencyProperty BlurVisibilityProperty;

        static BluredTextButton()
        {
            BluredForegroundProperty = DependencyProperty.Register("BluredForeground", typeof(Brush), typeof(BluredTextButton), new UIPropertyMetadata(Brushes.Blue));
            BlurVisibilityProperty = DependencyProperty.Register("BlurVisibility", typeof(Visibility), typeof(BluredTextButton), new UIPropertyMetadata(Visibility.Hidden));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(BluredTextButton), new FrameworkPropertyMetadata(typeof(BluredTextButton)));
        }

        public Brush BluredForeground { get; set; }
        public Visibility BlurVisibility { get; set; }
    }
}
