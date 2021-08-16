using ElectronicQueue.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace ElectronicQueue.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainWindowVM(this);

            InitializeComponent();
        }

        private void UpperPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
