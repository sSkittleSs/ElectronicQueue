using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ElectronicQueue.Controls
{
    class IconButton : Button
    {
        public static readonly DependencyProperty ColorProperty;
        public static readonly DependencyProperty DarkenedColorProperty;
        public static readonly DependencyProperty BackgroundColorProperty;
        public static readonly DependencyProperty DarkenedBackgroundColorProperty;


        // #FFDA3B00
        static IconButton()
        {
            ColorProperty = DependencyProperty.Register("Color", typeof(Brush), typeof(IconButton), new UIPropertyMetadata(Brushes.Black));
            DarkenedColorProperty = DependencyProperty.Register("DarkenedColor", typeof(Brush), typeof(IconButton), new UIPropertyMetadata(Brushes.Black));
            BackgroundColorProperty = DependencyProperty.Register("BackgroundColor", typeof(Brush), typeof(IconButton), new UIPropertyMetadata(Brushes.White));
            DarkenedBackgroundColorProperty = DependencyProperty.Register("DarkenedBackgroundColor", typeof(Brush), typeof(IconButton), new UIPropertyMetadata(Brushes.White));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(IconButton), new FrameworkPropertyMetadata(typeof(IconButton)));
        }

        public Brush Color { get; set; }
        public Brush DarkenedColor { get; set; }
        public Brush BackgroundColor { get; set; }
        public Brush DarkenedBackgroundColor { get; set; }
    }
}
