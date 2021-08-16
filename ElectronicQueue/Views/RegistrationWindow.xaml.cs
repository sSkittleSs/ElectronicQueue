using ElectronicQueue.ViewModels;
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
using System.Windows.Shapes;

namespace ElectronicQueue.Views
{
    /// <summary>
    /// Логика взаимодействия для RegistrationWindow.xaml
    /// </summary>
    public partial class RegistrationWindow : UserControl
    {
        public RegistrationWindow()
        {
            InitializeComponent();

            DataContext = new RegistrationWindowVM();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                if (passwordBox.Name == "pwdBox")
                {
                    if (passwordBox.Password.Length > 0)
                        wtrMarkBox.Visibility = Visibility.Collapsed;
                    else
                        wtrMarkBox.Visibility = Visibility.Visible;
                }
                else
                {
                    if (passwordBox.Password.Length > 0)
                        wtrMarkBoxConfirm.Visibility = Visibility.Collapsed;
                    else
                        wtrMarkBoxConfirm.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
