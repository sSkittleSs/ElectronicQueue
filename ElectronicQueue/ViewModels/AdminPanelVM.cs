using ElectronicQueue.DataBase.Models;
using ElectronicQueue.DataBase.Models.GridModels;
using ElectronicQueue.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicQueue.ViewModels
{
    class AdminPanelVM : BaseVM
    {
        #region Fields
        private ObservableCollection<object> items;
        private int selectedItem = 1;

        private DateTime selectedDate = DateTime.Today;
        private string hoursStart;
        private string minutesStart;
        private string hoursEnd;
        private string minutesEnd;
        private string step;

        private ObservableCollection<UserGridItem> users = new ObservableCollection<UserGridItem>();
        private ObservableCollection<SpecialityGridItem> specialities = new ObservableCollection<SpecialityGridItem>();
        private ObservableCollection<CabinetGridItem> cabinets = new ObservableCollection<CabinetGridItem>();
        private ObservableCollection<DoctorGridItem> doctors = new ObservableCollection<DoctorGridItem>();

        private UserGridItem selectedUser;
        private SpecialityGridItem selectedSpeciality;
        private CabinetGridItem selectedCabinet;
        private DoctorGridItem selectedDoctor;
        #endregion

        #region Properties
        public ObservableCollection<object> Items
        {
            set => SetProperty(ref items, value);
            get => items;
        }

        public int SelectedItem
        {
            set => SetProperty(ref selectedItem, value);
            get => selectedItem;
        }

        public DateTime SelectedDate
        {
            set => SetProperty(ref selectedDate, value);
            get => selectedDate;
        }

        public string HoursStart
        {
            set
            {
                if ((!int.TryParse(value, out int result) || result >= 24 || result < 0) && value != string.Empty)
                    return;

                hoursStart = value;
            }
            get => hoursStart;
        }

        public string MinutesStart
        {
            set
            {
                if ((!int.TryParse(value, out int result) || result >= 60 || result < 0) && value != string.Empty)
                    return;

                minutesStart = value;
            }
            get => minutesStart;
        }

        public string HoursEnd
        {
            set
            {
                if ((!int.TryParse(value, out int result) || result >= 24 || result < 0) && value != string.Empty)
                    return;

                hoursEnd = value;
            }
            get => hoursEnd;
        }

        public string MinutesEnd
        {
            set
            {
                if ((!int.TryParse(value, out int result) || result >= 60 || result < 0) && value != string.Empty)
                    return;

                minutesEnd = value;
            }
            get => minutesEnd;
        }

        public string Step
        {
            set
            {
                if ((!int.TryParse(value, out int result) || result >= 60 || result < 0) && value != string.Empty)
                    return;

                step = value;
            }
            get => step;
        }

        public ObservableCollection<UserGridItem> Users
        {
            set => SetProperty(ref users, value);
            get => users;
        }

        public ObservableCollection<SpecialityGridItem> Specialities
        {
            set => SetProperty(ref specialities, value);
            get => specialities;
        }

        public ObservableCollection<CabinetGridItem> Cabinets
        {
            set => SetProperty(ref cabinets, value);
            get => cabinets;
        }

        public ObservableCollection<DoctorGridItem> Doctors
        {
            set => SetProperty(ref doctors, value);
            get => doctors;
        }

        public UserGridItem SelectedUser
        {
            set => SetProperty(ref selectedUser, value);
            get => selectedUser;
        }

        public SpecialityGridItem SelectedSpeciality
        {
            set => SetProperty(ref selectedSpeciality, value);
            get => selectedSpeciality;
        }

        public CabinetGridItem SelectedCabinet
        {
            set => SetProperty(ref selectedCabinet, value);
            get => selectedCabinet;
        }

        public DoctorGridItem SelectedDoctor
        {
            set => SetProperty(ref selectedDoctor, value);
            get => selectedDoctor;
        }
        #endregion

        #region Constructors
        public AdminPanelVM()
        {
            OpenUsers.Execute();
        }
        #endregion

        #region Commands / Methods
        public DelegateCommand AddTickets => new DelegateCommand(() =>
        {
            if(IsEmptyDataForTickets())
            {
                NotificationManager.ShowWarning("Сначала заполните все поля!");
                return;
            }

            var startDate = SelectedDate;
            startDate = startDate.AddHours(int.Parse(HoursStart));
            startDate = startDate.AddMinutes(int.Parse(MinutesStart));

            var endDate = SelectedDate;
            endDate = endDate.AddHours(int.Parse(HoursEnd));
            endDate = endDate.AddMinutes(int.Parse(MinutesEnd));

            if (startDate > endDate)
            {
                NotificationManager.ShowWarning("Время начала периода не может превышать время конца периода!");
                return;
            }
            
            var ticketsForThisDate = MainWindowVM.DataBase.Tickets.Where((ticket) => ticket.DoctorID == SelectedDoctor.DoctorId)?.ToList();

            foreach(var item in new List<Ticket>(ticketsForThisDate))
                if (DateConverter.StringToDateTime(item.DateTime).Date != SelectedDate.Date
                        || DateConverter.StringToDateTime(item.DateTime) < startDate
                        || DateConverter.StringToDateTime(item.DateTime) > endDate)
                    ticketsForThisDate.Remove(item);

            if (ticketsForThisDate.Count() > 0)
            {
                NotificationManager.ShowError("На текущий промежуток времени уже есть как минимум один талон!");
                return;
            }

            int ticketsCount = (int.Parse(HoursEnd) * 60 + int.Parse(MinutesEnd) - int.Parse(HoursStart) * 60 - int.Parse(MinutesStart)) / int.Parse(Step);


            for (int i = 0; i < ticketsCount; i++)
            {
                var newDate = startDate;
                newDate = newDate.AddMinutes(i * int.Parse(Step));
                Items.Add(new Ticket(DateConverter.DateTimeToString(newDate), SelectedDoctor.DoctorId, null));
            }

            NotificationManager.ShowSuccess($"Талоны ({ticketsCount} шт.) успешно добавлены в базу данных.\nНе забудьте сохранить изменения!");

        });

        public DelegateCommand AddDoctor => new DelegateCommand(() =>
        {
            if (SelectedUser == default || SelectedCabinet == default || SelectedSpeciality == default)
            {
                NotificationManager.ShowWarning("Сначала заполните все поля!");
                return;
            }

            try
            {
                if (MessageBox.Show($"Вы желаете добавить нового доктора ({SelectedUser})?", "Внимание", button: MessageBoxButton.YesNo, icon: MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    MainWindowVM.DataBase.Doctors.Add(new Doctor(SelectedUser.UserId, SelectedSpeciality.SpecialityId, SelectedCabinet.CabinetId));
                    var user = MainWindowVM.DataBase.Users.FirstOrDefault((us) => us.UserID == SelectedUser.UserId);
                    user.RoleID = 2;
                    NotificationManager.ShowSuccess($"Доктор {user.Name} успешно добавлен в базу данных.\nНе забудьте сохранить изменения!");
                    //MainWindowVM.DataBase.SaveChangesAsync().Await(() => NotificationManager.ShowSuccess("Изменения успешно сохранены!"));
                }
            }
            catch (Exception e) { NotificationManager.ShowError(e.Message); }
        });

        public DelegateCommand OpenUsers => new DelegateCommand(() =>
        {
            Items = new ObservableCollection<object>(MainWindowVM.DataBase.Users);
            SelectedItem = 1;
        });

        public DelegateCommand OpenTickets => new DelegateCommand(() =>
        {
            Doctors?.Clear();

            foreach (var item in MainWindowVM.DataBase.Doctors)
                Doctors.Add(new DoctorGridItem(item.DoctorID, MainWindowVM.DataBase.Users.FirstOrDefault((user) => user.UserID == item.UserID).Name));

            Items = new ObservableCollection<object>(MainWindowVM.DataBase.Tickets);
            SelectedItem = 2;
        });

        public DelegateCommand OpenDoctors => new DelegateCommand(() =>
        {
            Users?.Clear();
            Cabinets?.Clear();
            Specialities?.Clear();

            foreach (var item in MainWindowVM.DataBase.Users.Where((user) => user.RoleID == 1))
                Users.Add(new UserGridItem(item.UserID, item.Name));

            foreach (var item in MainWindowVM.DataBase.Cabinets)
                Cabinets.Add(new CabinetGridItem(item.CabinetID, item.Number.ToString()));

            foreach (var item in MainWindowVM.DataBase.Specialities)
                Specialities.Add(new SpecialityGridItem(item.SpecialityID, item.Name));

            Items = new ObservableCollection<object>(MainWindowVM.DataBase.Doctors);
            SelectedItem = 3;
        });

        public DelegateCommand OpenSpecialities => new DelegateCommand(() =>
        {
            Items = new ObservableCollection<object>(MainWindowVM.DataBase.Specialities);
            SelectedItem = 4;
        });

        public DelegateCommand OpenRoles => new DelegateCommand(() =>
        {
            Items = new ObservableCollection<object>(MainWindowVM.DataBase.Roles);
            SelectedItem = 5;
        });

        public DelegateCommand OpenPolyclinics => new DelegateCommand(() =>
        {
            Items = new ObservableCollection<object>(MainWindowVM.DataBase.Polyclinics);
            SelectedItem = 6;
        });

        public DelegateCommand OpenCabinets => new DelegateCommand(() =>
        {
            Items = new ObservableCollection<object>(MainWindowVM.DataBase.Cabinets);
            SelectedItem = 7;
        });

        public DelegateCommand OpenAdresses => new DelegateCommand(() =>
        {
            Items = new ObservableCollection<object>(MainWindowVM.DataBase.Adreses);
            SelectedItem = 8;
        });

        public DelegateCommand SaveChanges => new DelegateCommand(() =>
        {
            try
            {
                if (MessageBox.Show("Вы желаете сохранить изменения?", "Внимание", button: MessageBoxButton.YesNo, icon: MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    UpdateDataBaseTableIfValuesChanged(SelectedItem);
                    MainWindowVM.DataBase.SaveChangesAsync().Await(() => NotificationManager.ShowSuccess("Изменения успешно сохранены!"));
                }
            }
            catch (Exception e) { NotificationManager.ShowError(e.Message); }
        });

        public DelegateCommand Update => new DelegateCommand(() =>
        {
            var switchedGrid = "";

            MainWindowVM.DataBase = new DataBase.ApplicationContext();

            switch(SelectedItem)
            {
                case 1: OpenUsers.Execute(); switchedGrid = "Пользователи"; break;
                case 2: OpenTickets.Execute(); switchedGrid = "Билеты";  break;
                case 3: OpenDoctors.Execute(); switchedGrid = "Доктора";  break;
                case 4: OpenSpecialities.Execute(); switchedGrid = "Специальности"; break;
                case 5: OpenRoles.Execute(); switchedGrid = "Роли"; break;
                case 6: OpenPolyclinics.Execute(); switchedGrid = "Поликлиники"; break;
                case 7: OpenCabinets.Execute(); switchedGrid = "Кабинеты"; break;
                case 8: OpenAdresses.Execute(); switchedGrid = "Адреса"; break;
                default: NotificationManager.ShowError("Ошибка индекса!"); return;
            }
            
            NotificationManager.ShowSuccess($"Таблица '{switchedGrid}' успешно обновлена к значениям из БД!");
        });

        private void UpdateDataBaseTableIfValuesChanged(int tableId)
        {
            switch(tableId)
            {
                case 1: UpdateUsers(); break;
                case 2: UpdateTickets(); break;
                case 3: UpdateDoctors(); break;
                case 4: UpdateSpecialities(); break;
                case 5: UpdateRoles(); break;
                case 6: UpdatePolyclinics(); break;
                case 7: UpdateCabinets(); break;
                case 8: UpdateAdresses(); break;
                default: NotificationManager.ShowError("Не удалось добавить новые записи в таблицу."); break;
            }
        }

        private void UpdateUsers()
        {
            if (Items.Count == MainWindowVM.DataBase.Users.Count())
                return;

            for (int i = MainWindowVM.DataBase.Users.Count(); i < Items.Count; i++)
                if (Items[i] is User value)
                    MainWindowVM.DataBase.Users.Add(value);
        }

        private void UpdateTickets()
        {
            if (Items.Count == MainWindowVM.DataBase.Tickets.Count())
                return;

            for (int i = MainWindowVM.DataBase.Tickets.Count(); i < Items.Count; i++)
                if (Items[i] is Ticket value)
                    MainWindowVM.DataBase.Tickets.Add(value);
        }

        private void UpdateDoctors()
        {
            if (Items.Count == MainWindowVM.DataBase.Doctors.Count())
                return;

            for (int i = MainWindowVM.DataBase.Doctors.Count(); i < Items.Count; i++)
                if (Items[i] is Doctor value)
                    MainWindowVM.DataBase.Doctors.Add(value);
        }

        private void UpdateSpecialities()
        {
            if (Items.Count == MainWindowVM.DataBase.Specialities.Count())
                return;

            for (int i = MainWindowVM.DataBase.Specialities.Count(); i < Items.Count; i++)
                if (Items[i] is Speciality value)
                    MainWindowVM.DataBase.Specialities.Add(value);
        }

        private void UpdateRoles()
        {
            if (Items.Count == MainWindowVM.DataBase.Roles.Count())
                return;

            for (int i = MainWindowVM.DataBase.Roles.Count(); i < Items.Count; i++)
                if (Items[i] is Role value)
                    MainWindowVM.DataBase.Roles.Add(value);
        }

        private void UpdatePolyclinics()
        {
            if (Items.Count == MainWindowVM.DataBase.Polyclinics.Count())
                return;

            for (int i = MainWindowVM.DataBase.Polyclinics.Count(); i < Items.Count; i++)
                if (Items[i] is Polyclinic value)
                    MainWindowVM.DataBase.Polyclinics.Add(value);
        }

        private void UpdateCabinets()
        {
            if (Items.Count == MainWindowVM.DataBase.Cabinets.Count())
                return;

            for (int i = MainWindowVM.DataBase.Cabinets.Count(); i < Items.Count; i++)
                if (Items[i] is Cabinet value)
                    MainWindowVM.DataBase.Cabinets.Add(value);
        }

        private void UpdateAdresses()
        {
            if (Items.Count == MainWindowVM.DataBase.Adreses.Count())
                return;

            for (int i = MainWindowVM.DataBase.Adreses.Count(); i < Items.Count; i++)
                if (Items[i] is Adres value)
                    MainWindowVM.DataBase.Adreses.Add(value);
        }

        private bool IsEmptyDataForTickets() => HoursStart == string.Empty || HoursEnd == string.Empty || MinutesStart == string.Empty || MinutesEnd == string.Empty || SelectedDoctor == default || Step == string.Empty;
        #endregion
    }
}
