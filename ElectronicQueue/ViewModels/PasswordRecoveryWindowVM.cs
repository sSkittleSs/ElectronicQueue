using ElectronicQueue.Models;
using ElectronicQueue.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ElectronicQueue.ViewModels
{
    class PasswordRecoveryWindowVM : BaseVM
    {
        #region Fields
        private Brush borderBrush = Brushes.Blue;
        private string email;
        #endregion

        #region Properties
        public Brush BorderBrush
        {
            set => SetProperty(ref borderBrush, value);
            get => borderBrush;
        }

        public string Email
        {
            set
            {
                SetProperty(ref email, value);

                if (value.Length == 0)
                    BorderBrush = Brushes.Blue;
                else if (!MailValidator.IsValidEmail(Email))
                    BorderBrush = Brushes.Red;
                else
                    BorderBrush = Brushes.Green;
            }
            get => email;
        }

        public PasswordRecoveryWindow ThisWindow { set; get; }
        #endregion
        public PasswordRecoveryWindowVM(PasswordRecoveryWindow thisWindow)
        {
            ThisWindow = thisWindow;
        }
        #region Constructors

        #endregion

        #region Commands / Methods
        public DelegateCommand Exit => new DelegateCommand(() =>
        {
            ThisWindow.DialogResult = false;
            ThisWindow.Close();
        });

        public DelegateCommand Recovery => new DelegateCommand(() =>
        {
            if (MainWindowVM.DataBase.Users.FirstOrDefault((user) => user.Email == Email) != default)
            {
                MailManager.SendPasswordRecoveryLetter(Email);
                ThisWindow.DialogResult = true;
                ThisWindow.Close();
            }
            else
            {
                NotificationManager.ShowWarning("Аккаунта с такой почтой не существует!");
            }
        });
        #endregion
    }
}
