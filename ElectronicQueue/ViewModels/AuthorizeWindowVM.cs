using ElectronicQueue.Models;
using ElectronicQueue.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicQueue.ViewModels
{
    class AuthorizeWindowVM : BaseVM
    {
        #region Fields
        private MainWindow mainWindow;
        private string email;
        private PasswordBox passwordBox;
        #endregion

        #region Properties
        public MainWindow MainWindow
        {
            set => SetProperty(ref mainWindow, value);
            get => mainWindow;
        }

        public string Email
        {
            set => SetProperty(ref email, value);
            get => email;
        }

        public PasswordBox PasswordBox
        {
            set => SetProperty(ref passwordBox, value);
            get => passwordBox;
        }
        #endregion

        #region Constructors
        public AuthorizeWindowVM()
        {
            MainWindow = MainWindowVM.MainWindow;
        }
        #endregion

        #region Commands / Methods
        public DelegateCommand Authorize => new DelegateCommand(() =>
        {
            // Проверяем почту на пустое значение
            if (Email == null)
            {
                NotificationManager.ShowWarning("Почта не введена!"); // Выводим уведомление
                return;
            }

            UpdatePasswordBox(); // Получаем текущие данные о введенном пароле
            if (PasswordBox.Password == null)
            {
                NotificationManager.ShowWarning("Пароль не введен!");
                return;
            }

            DataBase.Models.User user = null;
            if (MailValidator.IsValidEmail(Email)) // Если в поле логина введена почта, то выполняем поиск по почте и паролю
            {
                try
                {
                    user = MainWindowVM.DataBase.Users.FirstOrDefault((u) => u.Email == Email && u.Password == PasswordBox.Password);
                }
                catch (Exception e) { NotificationManager.ShowError(e.Message); }
            }
            else // иначе ищем аккаунт по логину
            {
                try
                {
                    // Если количество пользователей с таким логином меньше двух, то ищем аккаунт
                    if (MainWindowVM.DataBase.Users.Count((us) => us.Username == Email) < 2)
                        user = MainWindowVM.DataBase.Users.FirstOrDefault((u) => u.Username == Email && u.Password == PasswordBox.Password);
                    else // иначе выводим предупреждение
                        NotificationManager.ShowWarning("Вход по логину невозможен, поскольку найдено несколько человек с одинаковым логином.");
                }
                catch (Exception e) { MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Warning); }
            }

            // Если пользователь найден, то авторизируем его
            if (user != null)
            {
                try
                {
                    MainWindowVM.User = user;
                    ((MainWindowVM)MainWindow.DataContext).Authorize();
                    
                    ((MainWindowVM)MainWindow.DataContext).IsAdmin = user.RoleID == 3;

                    NotificationManager.ShowSuccess("Вы успешно авторизованы в системе!");
                }
                catch (Exception e) { NotificationManager.ShowError(e.Message); }
            }
            else // Иначе выдаем уведомление
            {
                NotificationManager.ShowError("Неверный логин или пароль!");
            }
        });

        public DelegateCommand PasswordRecovery => new DelegateCommand(() => { new PasswordRecoveryWindow().ShowDialog(); });
        public DelegateCommand OpenRegistation => new DelegateCommand(() => { ((MainWindowVM)MainWindow.DataContext).OpenRegistation.Execute(); });

        public void UpdatePasswordBox()
        {
            try 
            {
                if (PasswordBox == null) 
                    PasswordBox = ((AuthorizeWindow)((MainWindowVM)MainWindow.DataContext).CurrentView).pwdBox;
            }
            catch (Exception e) { MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Warning); }
        }
        #endregion
    }
}
