using System.Windows;
using System.Windows.Controls;

namespace ElectronicQueue.Controls
{
    public class ItemButton : Button
    {
        public static readonly DependencyProperty ActiveLineProperty;
        public static readonly DependencyProperty IdProperty;

        static ItemButton()
        {
            ActiveLineProperty = DependencyProperty.Register("ActiveLine", typeof(Visibility), typeof(ItemButton), new UIPropertyMetadata(Visibility.Hidden));
            IdProperty = DependencyProperty.Register("Id", typeof(string), typeof(ItemButton), new UIPropertyMetadata("0"));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(ItemButton), new FrameworkPropertyMetadata(typeof(ItemButton)));
        }

        public Visibility ActiveLine { get; set; }
        public string Id { get; set; }
    }
}
