using ElectronicQueue.DataBase.Models;
using ElectronicQueue.Models;
using ElectronicQueue.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicQueue.ViewModels
{
    class UCPWindowVM : BaseVM
    {
        #region Fields
        private string name;
        private string phoneNumber;
        private string email;
        private string password;
        private bool isReadOnlyPhoneNumber;
        private bool isReadOnlyEmail;
        private bool isReadOnlyPassword;
        private List<UCPGridItem> items;
        #endregion

        #region Properties
        public string Name
        {
            set => SetProperty(ref name, value);
            get => name;
        }

        public string PhoneNumber
        {
            set => SetProperty(ref phoneNumber, value);
            get => phoneNumber;
        }

        public string Email
        {
            set => SetProperty(ref email, value);
            get => email;
        }

        public string Password
        {
            set => SetProperty(ref password, value);
            get => password;
        }

        public bool IsReadOnlyPhoneNumber
        {
            set => SetProperty(ref isReadOnlyPhoneNumber, value);
            get => isReadOnlyPhoneNumber;
        }

        public bool IsReadOnlyEmail
        {
            set => SetProperty(ref isReadOnlyEmail, value);
            get => isReadOnlyEmail;
        }

        public bool IsReadOnlyPassword
        {
            set => SetProperty(ref isReadOnlyPassword, value);
            get => isReadOnlyPassword;
        }

        public List<UCPGridItem> Items
        {
            set => SetProperty(ref items, value);
            get => items;
        }
        #endregion

        #region Constructors
        public UCPWindowVM() 
        {
            Items = new List<UCPGridItem>();
            Name = MainWindowVM.User.Name;
            Email = MainWindowVM.User.Email;
            PhoneNumber = string.Format("{0:+###(##)###-##-##}", MainWindowVM.User.PhoneNumber);
            Password = MainWindowVM.User.Password;

            IsReadOnlyPhoneNumber = true;
            IsReadOnlyEmail = true;
            IsReadOnlyPassword = true;

            foreach (var item in MainWindowVM.DataBase.Tickets.Where((ticket) => ticket.UserID == MainWindowVM.User.UserID).ToList())
                if (DateConverter.StringToDateTime(item.DateTime) >= DateTime.Today)
                    Items.Add(new UCPGridItem(item.DateTime, MainWindowVM.DataBase.Specialities.FirstOrDefault((spec) => spec.SpecialityID == MainWindowVM.DataBase.Doctors.FirstOrDefault((doc) => doc.DoctorID == item.DoctorID).SpecialityID).Name));
        }
        #endregion

        #region Commands / Methods
        //public DelegateCommand EditPhoneNumber => new DelegateCommand(() => 
        //{
        //    if (!IsReadOnlyPhoneNumber && MessageBox.Show("Вы желаете сохранить изменения?", "Внимание", button: MessageBoxButton.YesNo, icon: MessageBoxImage.Warning) == MessageBoxResult.Yes)
        //    {
        //        IsReadOnlyPhoneNumber = true;

        //        MainWindowVM.DataBase.Users.Find(MainWindowVM.User.UserID).PhoneNumber = Convert.ToInt64(PhoneNumber);
        //        MainWindowVM.DataBase.SaveChangesAsync().Await(() => MessageBox.Show("Изменения успешно сохранены!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information));
        //    }
        //    else
        //    {
        //        IsReadOnlyPhoneNumber = false;
        //        MessageBox.Show("Разрешено редактирование номера телефона.\n\nПосле завершения редактирования обязательно\nнажмите на кнопку редактирования для сохранения изменений.", "Важно", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //});

        public DelegateCommand EditEmail => new DelegateCommand(() =>
        {
            if (!IsReadOnlyEmail && MessageBox.Show("Вы желаете сохранить изменения?", "Внимание", button: MessageBoxButton.YesNo, icon: MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                var user = MainWindowVM.DataBase.Users.Find(MainWindowVM.User.UserID);
                if (MailValidator.IsValidEmail(Email))
                {
                    if (MainWindowVM.DataBase.Users.Where((us) => us.Email == Email).Count() == 0)
                    {
                        if (new EmailConfirmationWindow(Email).ShowDialog() ?? false)
                        {
                            
                            user.Email = Email;
                            MainWindowVM.DataBase.SaveChangesAsync().Await(() => NotificationManager.ShowSuccess("Изменения успешно сохранены!"));
                            //MessageBox.Show("Полученные данные из БД\n\n" + MainWindowVM.DataBase.Users.FirstOrDefault((us) => us.Email == Email)?.ToString() ?? "NONE");
                        }
                        else
                        {
                            Email = user.Email;
                            NotificationManager.ShowError("Код подтверждения не был введен. Почта не изменена!");
                        }
                    }
                    else
                    {
                        Email = user.Email;
                        NotificationManager.ShowWarning("Данная почта уже используется другим пользователем!");
                    }
                }
                else
                {
                    Email = user.Email;
                    NotificationManager.ShowError("Почта имеет некорректный формат!\n\nИзменения не сохранены.");
                    //MessageBox.Show("Почта имеет некорректный формат!\n\nИзменения не сохпанены.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                IsReadOnlyEmail = true;
            }
            else
            {
                IsReadOnlyEmail = false;
                NotificationManager.ShowInformation("Разрешено редактирование почты.\n\nПосле завершения редактирования обязательно нажмите на кнопку редактирования для сохранения изменений.");
                //MessageBox.Show("Разрешено редактирование почты.\n\nПосле завершения редактирования обязательно\nнажмите на кнопку редактирования для сохранения изменений.", "Важно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        });

        public DelegateCommand EditPassword => new DelegateCommand(() =>
        {
            if (!IsReadOnlyPassword && MessageBox.Show("Вы желаете сохранить изменения?", "Внимание", button: MessageBoxButton.YesNo, icon: MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                IsReadOnlyPassword = true;
                MainWindowVM.DataBase.Users.Find(MainWindowVM.User.UserID).Password = Password;
                MainWindowVM.DataBase.SaveChangesAsync().Await(() => NotificationManager.ShowSuccess("Изменения успешно сохранены!"));
            }
            else
            {
                IsReadOnlyPassword = false;
                NotificationManager.ShowInformation("Разрешено редактирование пароля.\n\nПосле завершения редактирования обязательно нажмите на кнопку редактирования для сохранения изменений.");
                //MessageBox.Show("Разрешено редактирование пароля.\n\nПосле завершения редактирования обязательно\nнажмите на кнопку редактирования для сохранения изменений.", "Важно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        });
        #endregion
    }
}
