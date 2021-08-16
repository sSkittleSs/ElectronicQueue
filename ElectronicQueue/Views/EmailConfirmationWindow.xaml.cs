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
    /// Логика взаимодействия для EmailConfirmationWindow.xaml
    /// </summary>
    public partial class EmailConfirmationWindow : Window
    {
        public EmailConfirmationWindow(string email)
        {
            InitializeComponent();

            DataContext = new EmailConfirmationWindowVM(email, this);
        }
    }
}
