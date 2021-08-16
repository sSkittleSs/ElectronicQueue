using ElectronicQueue.Models;
using ElectronicQueue.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ElectronicQueue.ViewModels
{
    class EmailConfirmationWindowVM : BaseVM
    {
        #region Fields
        private Brush borderBrush = Brushes.Blue;
        private string email;
        private string confirmationCode;
        #endregion

        #region Properties
        public Brush BorderBrush
        {
            set => SetProperty(ref borderBrush, value);
            get => borderBrush;
        }

        public string Email
        {
            set => SetProperty(ref email, value);
            get => email;
        }

        public string ConfirmationCode
        {
            set
            {
                if (value.Length > 8)
                    return;

                char[] charValue = value.ToCharArray();

                for (int i = 0; i < charValue.Length; i++)
                {
                    if (!char.IsDigit(charValue[i]) && !char.IsLetter(charValue[i]))
                        return;
                    else if (char.IsLower(charValue[i]))
                        charValue[i] = char.ToUpper(charValue[i]);
                }

                SetProperty(ref confirmationCode, new string(charValue));

                if (charValue.Length == 0)
                    BorderBrush = Brushes.Blue;
                else if (charValue.Length != 8)
                    BorderBrush = Brushes.Red;
                else
                    BorderBrush = Brushes.Green;
            }
            get => confirmationCode;
        }

        public string CorrectConfirmationCode { set; get; }
        public EmailConfirmationWindow ThisWindow { set; get; }
        #endregion
        public EmailConfirmationWindowVM(string email, EmailConfirmationWindow thisWindow)
        {
            Email = email;
            ThisWindow = thisWindow;
            CorrectConfirmationCode = MailManager.SendConfirmationLetter(email);
        }
        #region Constructors

        #endregion

        #region Commands / Methods
        public DelegateCommand ResendLetter => new DelegateCommand(() =>
        {
            CorrectConfirmationCode = MailManager.SendConfirmationLetter(Email);
        });

        public DelegateCommand Exit => new DelegateCommand(() =>
        {
            ThisWindow.DialogResult = false;
            ThisWindow.Close();
        });

        public DelegateCommand Confirm => new DelegateCommand(() =>
        {
            if (ConfirmationCode == CorrectConfirmationCode)
            {
                ThisWindow.DialogResult = true;
                ThisWindow.Close();
            }
            else
            {
                NotificationManager.ShowError("Некорректный код подтверждения!");
            }
        });
        #endregion
    }
}
