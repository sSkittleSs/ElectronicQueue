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
    class RegistrationWindowVM : BaseVM
    {
        #region Fields
        private MainWindow mainWindow;
        private string name;
        private DateTime? birthday;
        private string login;
        private string email;
        private string phoneNumber;
        private PasswordBox passwordBox;
        private PasswordBox passwordBoxConfrim;
        #endregion

        #region Properties
        public MainWindow MainWindow
        {
            set => SetProperty(ref mainWindow, value);
            get => mainWindow;
        }

        public string Name
        {
            set => SetProperty(ref name, value);
            get => name;
        }

        public DateTime? Birthday
        {
            set => SetProperty(ref birthday, value);
            get => birthday;
        }

        public string Login
        {
            set => SetProperty(ref login, value);
            get => login;
        }

        public string Email
        {
            set => SetProperty(ref email, value);
            get => email;
        }

        public string PhoneNumber
        {
            set
            {
                if (value.Length > 12)
                    return;

                if (value == string.Empty)
                {
                    SetProperty(ref phoneNumber, null);
                    return;
                }

                if (Int64.TryParse(value, out long res))
                    SetProperty(ref phoneNumber, value);
            }
            get => phoneNumber;
        }

        public PasswordBox PasswordBox
        {
            set => SetProperty(ref passwordBox, value);
            get => passwordBox;
        }

        public PasswordBox PasswordBoxConfirm
        {
            set => SetProperty(ref passwordBoxConfrim, value);
            get => passwordBoxConfrim;
        }
        #endregion

        #region Constructors
        public RegistrationWindowVM()
        {
            MainWindow = MainWindowVM.MainWindow;
        }
        #endregion

        #region Commands / Methods
        public DelegateCommand Registration => new DelegateCommand(() =>
        {
            // Проверяем поле ФИО на пустоту
            if (Name == null || Name == string.Empty)
            {
                NotificationManager.ShowWarning("ФИО некорректно!");
                return;
            }

            // Проверяем поле дня рождения на корректность
            if (Birthday == null || Birthday >= DateTime.Today)
            {
                NotificationManager.ShowWarning("Дата рождения некорректна!");
                return;
            }

            // Проверяем поле логина на пустоту
            if (Login == null || Login == string.Empty)
            {
                NotificationManager.ShowWarning("Некорректный логин!");
                return;
            }

            // Проверяем поле почты на корректность
            if (!MailValidator.IsValidEmail(Email))
            {
                NotificationManager.ShowWarning($"Почта {Email} имеет некорректный формат!");
                return;
            }

            // Проверяем поле номера телефона на пустоту и количество символов
            if (PhoneNumber == null || PhoneNumber == string.Empty || PhoneNumber.Length != 12)
            {
                NotificationManager.ShowWarning("Некорректный номер телефона (формат \"XXXYYDDDDDDD\",\nгде XXX - код страны, YY - код оператора, DDDDDDD - номер телефона)!");
                return;
            }

            UpdatePasswordBoxes(); // Получаем последние данных из элементов ввода пароля
            if (PasswordBox.Password.Length < 6) // Проверяем длину пароля на количество символов
            {
                NotificationManager.ShowWarning("Длина пароля не может быть меньше 6");
                return;
            }

            // Проверяем совпадение паролей
            if (PasswordBox.Password != PasswordBoxConfirm.Password)
            {
                NotificationManager.ShowWarning("Пароли не совпадают!");
                return;
            }

            try 
            { 
                // Ищем пользователей с такой же почтой
                if (MainWindowVM.DataBase.Users.FirstOrDefault((us) => us.Email == Email) == null)
                {
                    // Если не нашли, тогда открываем диалоговое окно для подтверждения почты
                    if (new EmailConfirmationWindow(Email).ShowDialog() ?? false)
                    {
                        // Добавляем пользователя в БД
                        MainWindowVM.DataBase.Users.Add(new DataBase.Models.User(Name, Birthday?.ToString("yyyy-MM-dd HH:mm:ss") ?? "", Login, Email, Convert.ToInt64(PhoneNumber), PasswordBox.Password, 1));
                        MainWindowVM.DataBase.SaveChangesAsync().Await(() => 
                        {
                            NotificationManager.ShowSuccess("Вы успешно зарегистрированы в системе!");
                            ((MainWindowVM)MainWindowVM.MainWindow.DataContext).OpenAuthorize.Execute();
                        });
                    }
                    else // Если код неверный - выдаем ошибку
                    {
                        NotificationManager.ShowError("Код подтверждения не был введен. Пользователь не зарегистрирован!");
                    }
                }
                else // Если пользователь уже существует
                {
                    NotificationManager.ShowWarning("Пользователь с такой почтой уже зарегистрирован!");
                }
            }
            catch (Exception e) { NotificationManager.ShowError(e.Message); }
        });

        public void UpdatePasswordBoxes()
        {
            try
            {
                if (PasswordBox == null)
                    PasswordBox = ((RegistrationWindow)((MainWindowVM)MainWindow.DataContext).CurrentView).pwdBox;

                if (PasswordBoxConfirm == null)
                    PasswordBoxConfirm = ((RegistrationWindow)((MainWindowVM)MainWindow.DataContext).CurrentView).pwdBoxConfirm;
            }
            catch (Exception e) { NotificationManager.ShowError(e.Message); }
        }
        #endregion
    }
}
