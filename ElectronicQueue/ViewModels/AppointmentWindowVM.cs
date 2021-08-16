using ElectronicQueue.Models;
using ElectronicQueue.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ElectronicQueue.DataBase.Models;
using System.Windows.Media;
using System.Diagnostics;

namespace ElectronicQueue.ViewModels
{
    class AppointmentWindowVM : BaseVM
    {
        #region Fields
        private string name;
        private string phoneNumber;
        private string email;
        private string speciality;
        private bool isQueueAdmin;
        private object ticketSelectedItem;
        private object specialitySelectedItem;
        private DoctorName doctorSelectedItem;
        private readonly ObservableCollection<AppointmentGridItem> items;
        private readonly ObservableCollection<string> specialities;
        private readonly ObservableCollection<DoctorName> doctors;

        public readonly ReadOnlyObservableCollection<AppointmentGridItem> ItemsCollection;
        public readonly ReadOnlyObservableCollection<string> SpecialitiesCollection;
        public readonly ReadOnlyObservableCollection<DoctorName> DoctorsCollection;
        #endregion

        #region Properties
        public bool IsAdmin => ((MainWindowVM)MainWindowVM.MainWindow.DataContext).IsAdmin;

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

        public string Speciality
        {
            set => SetProperty(ref speciality, value);
            get => speciality;
        }

        public bool IsQueueAdmin
        {
            set => SetProperty(ref isQueueAdmin, value);
            get => isQueueAdmin;
        }

        public object TicketSelectedItem
        {
            set
            {
                SetProperty(ref ticketSelectedItem, value);

                var ind = ((DataGridTextColumn)((DataGridCellInfo)TicketSelectedItem).Column)?.DisplayIndex ?? -1;
                if (ind != -1)
                {
                    SelectedCellColumn = ind;
                    SelectedGridItem = (AppointmentGridItem)((DataGridCellInfo)TicketSelectedItem).Item;
                }
            }
            get => ticketSelectedItem;
        }

        public int SelectedCellColumn { set; get; }

        public AppointmentGridItem SelectedGridItem { set; get; }

        public object SpecialitySelectedItem
        {
            set
            {
                SetProperty(ref specialitySelectedItem, value);

                SetDoctorsBySpecialityID(MainWindowVM.DataBase.Specialities.FirstOrDefault((spec) => spec.Name == SpecialitySelectedItem.ToString())?.SpecialityID ?? 1);
            }
            get => specialitySelectedItem;
        }

        public DoctorName DoctorSelectedItem
        {
            set
            {
                SetProperty(ref doctorSelectedItem, value);

                var selectedItem = DoctorSelectedItem?.ToString();
                var userId = MainWindowVM.DataBase.Users.FirstOrDefault((us) => us.Name == selectedItem)?.UserID;
                var id = MainWindowVM.DataBase.Doctors.FirstOrDefault((doc) => doc.UserID == userId)?.DoctorID ?? 0;

                IsQueueAdmin = MainWindowVM.User.UserID == userId;
                SetInfoForDoctorByID(id);
                SetItemsForDoctorByID(id);

                if (double.Parse(DoctorSelectedItem?.Load ?? "0") >= 75)
                    NotificationManager.ShowWarning($"Данный врач нагружен на {DoctorSelectedItem?.Load ?? "ERROR"}%!\nВозможно вам стоит выбрать другого врача.");
            }
            get => doctorSelectedItem;
        }

        public ReadOnlyObservableCollection<AppointmentGridItem> Items => ItemsCollection;
        public ReadOnlyObservableCollection<string> Specialities => SpecialitiesCollection;
        public ReadOnlyObservableCollection<DoctorName> Doctors => DoctorsCollection;
        #endregion

        #region Constructors
        public AppointmentWindowVM()
        {
            items = new ObservableCollection<AppointmentGridItem>();
            specialities = new ObservableCollection<string>();
            doctors = new ObservableCollection<DoctorName>();

            ItemsCollection = new ReadOnlyObservableCollection<AppointmentGridItem>(items);
            SpecialitiesCollection = new ReadOnlyObservableCollection<string>(specialities);
            DoctorsCollection = new ReadOnlyObservableCollection<DoctorName>(doctors);

            SetInfoForDoctorByID(0);
            SetItemsForDoctorByID(0);

            foreach (var item in MainWindowVM.DataBase.Specialities)
                specialities.Add(item.Name);

        }
        #endregion

        #region Commands / Methods
        public DelegateCommand Add => new DelegateCommand(() =>
        {
            // Проверяем на то, является ли пользователь врачом-администратором очереди
            if (IsQueueAdmin)
            {
                NotificationManager.ShowError("Вы являетесь администратором очереди и не можете зарезервировать талон к самому себе!");
                return;
            }

            // Получаем id доктора, к которому мы записываемся
            var selectedDoctorItem = DoctorSelectedItem?.ToString();
            var doctorUserId = MainWindowVM.DataBase.Users.FirstOrDefault((us) => us.Name == selectedDoctorItem)?.UserID;
            var id = MainWindowVM.DataBase.Doctors.FirstOrDefault((doc) => doc.UserID == doctorUserId)?.DoctorID ?? 0;

            // Получаем время записи и ищем талон в базе данных
            var selectedTicketItem = SelectedCellColumn == 0 ? SelectedGridItem?.FreeDate : SelectedGridItem?.PrivatedDate;
            var selectedTicket = MainWindowVM.DataBase.Tickets.FirstOrDefault((ticket) => ticket.DateTime == selectedTicketItem && ticket.DoctorID == id);
            if (selectedTicket?.UserID != null) // Проверяем на то, зарезервирован ли талон другим пользователем
            {
                NotificationManager.ShowError("Талон уже зарезервирован другим человеком!");
                return;
            }

            // Ищем талоны на такое же время у текущего пользователя
            if (MainWindowVM.DataBase.Tickets.Where((ticket) => ticket.DateTime == selectedTicketItem && ticket.UserID == MainWindowVM.User.UserID).Count() > 0)
            {
                NotificationManager.ShowError("У вас уже есть талон на выбранное время!");
                return;
            }

            // Если выбран талон, тогда резервируем его за текущим пользователем
            if (selectedTicket != null)
            {
                selectedTicket.UserID = MainWindowVM.User.UserID;
                MainWindowVM.DataBase.SaveChangesAsync().Await(() => NotificationManager.ShowSuccess($"Талон на время {selectedTicket.DateTime} успешно зарезервирован, теперь вы можете просмотреть его в личном кабинете."));
                SetItemsForDoctorByID(id);

                SetDoctorsBySpecialityID(MainWindowVM.DataBase.Specialities.FirstOrDefault((spec) => spec.Name == SpecialitySelectedItem.ToString())?.SpecialityID ?? 1);
            }
            else // Иначе уведомляем об ошибке.
            {
                NotificationManager.ShowWarning("Не выбран талон!");
            }
        });

        public DelegateCommand Info => new DelegateCommand(() =>
        {
            var selectedDoctorItem = DoctorSelectedItem?.ToString();
            var doctorUserId = MainWindowVM.DataBase.Users.FirstOrDefault((us) => us.Name == selectedDoctorItem)?.UserID;
            var id = MainWindowVM.DataBase.Doctors.FirstOrDefault((doc) => doc.UserID == doctorUserId)?.DoctorID ?? 0;

            var selectedTicketItem = SelectedCellColumn == 0 ? SelectedGridItem?.FreeDate : SelectedGridItem?.PrivatedDate;
            var selectedTicket = MainWindowVM.DataBase.Tickets.FirstOrDefault((ticket) => ticket.DateTime == selectedTicketItem && ticket.DoctorID == id);
            if (selectedTicket?.UserID == null)
            {
                NotificationManager.ShowInformation("Выбранный талон никем не занят.");
                //MessageBox.Show("Выбранный талон никем не занят.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                var userId = MainWindowVM.DataBase.Tickets.FirstOrDefault((ticket) => ticket.TicketID == selectedTicket.TicketID);
                var user = MainWindowVM.DataBase.Users.FirstOrDefault((us) => us.UserID == userId.UserID);

                NotificationManager.ShowInformation($"  Информация о пациенте {user.Name}:\nE-mail: {user.Email}\nНомер тел.: {string.Format("{0:+###(##)###-##-##}", user.PhoneNumber)}");
                //MessageBox.Show($"  Информация о пациенте {user.Name}:\nE-mail: {user.Email}\nНомер тел.: {string.Format("{0:+###(##)###-##-##}", user.PhoneNumber)}", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        });

        public void SetInfoForDoctorByID(int doctorID)
        {
            var userID = Doctor.GetPropertyByDoctorID(doctorID, Doctor.DoctorProperties.UserID);
            Name = User.GetPropertyInStringByUserID(userID, User.UserProperties.Name);
            Email = User.GetPropertyInStringByUserID(userID, User.UserProperties.Email);
            PhoneNumber = string.Format("{0:+###(##)###-##-##}", MainWindowVM.DataBase.Users.FirstOrDefault((us) => us.UserID == userID)?.PhoneNumber);
            Speciality = DataBase.Models.Speciality.GetPropertyInStringBySpecialityID(Doctor.GetPropertyByDoctorID(doctorID, Doctor.DoctorProperties.SpecialityID), DataBase.Models.Speciality.SpecialityProperties.Name);
            
            //Name = MainWindowVM.DataBase.Users.FirstOrDefault((us) => us.UserID == MainWindowVM.DataBase.Doctors.FirstOrDefault((doc) => doc.DoctorID == doctorID).UserID)?.Name;
            //Email = MainWindowVM.DataBase.Users.FirstOrDefault((us) => us.UserID == MainWindowVM.DataBase.Doctors.FirstOrDefault((doc) => doc.DoctorID == doctorID).UserID)?.Email;
            //Speciality = MainWindowVM.DataBase.Specialities.FirstOrDefault((spec) => spec.SpecialityID == MainWindowVM.DataBase.Doctors.FirstOrDefault((doc) => doc.DoctorID == doctorID).SpecialityID)?.Name;
        }

        public void SetItemsForDoctorByID(int doctorId)
        {
            ObservableCollection<Ticket> freeTickets = new ObservableCollection<Ticket>();
            ObservableCollection<Ticket> privatedTickets = new ObservableCollection<Ticket>();
            
            if (items != null)
                items.Clear();

            foreach (var item in MainWindowVM.DataBase.Tickets.Where((ticket) => ticket.DoctorID == doctorId).ToList())
            {
                if (DateConverter.StringToDateTime(item.DateTime) >= DateTime.Today)
                {
                    if (item.UserID == null)
                        freeTickets.Add(item);
                    else
                        privatedTickets.Add(item);
                }
            }

            for (int i = 0; i < (freeTickets.Count >= privatedTickets.Count ? freeTickets.Count : privatedTickets.Count); i++)
            {
                if (i < freeTickets.Count && i < privatedTickets.Count)
                {
                    items.Add(new AppointmentGridItem(freeTickets[i].DateTime, privatedTickets[i].DateTime));
                }
                else
                {
                    if (i >= freeTickets.Count)
                        items.Add(new AppointmentGridItem("", privatedTickets[i].DateTime));
                    else
                        items.Add(new AppointmentGridItem(freeTickets[i].DateTime, ""));
                }
            }

            OnPropertyChanged(nameof(Items));
            OnPropertyChanged("FreeDate");
            OnPropertyChanged("PrivatedDate");
        }

        public void SetDoctorsBySpecialityID(int specialityID)
        {
            if (doctors != null)
                doctors.Clear();
            
            
            //var docs = MainWindowVM.DataBase.Doctors;
            foreach (var item in MainWindowVM.DataBase.Doctors.Where((doc) => doc.SpecialityID == specialityID))
            {
                var load = CalculateLoadForDoctorById(item.DoctorID);
                var loadColor = new SolidColorBrush(Color.FromRgb((byte)(int)(255 * load / 100), (byte)(int)(255 * (1 - load / 100)), 0));
                doctors?.Add(new DoctorName(User.GetPropertyInStringByUserID(Doctor.GetPropertyByDoctorID(item.DoctorID, Doctor.DoctorProperties.UserID), User.UserProperties.Name), load.ToString("0.##"), loadColor));
            }

            OnPropertyChanged(nameof(Doctors));
        }

        private double CalculateLoadForDoctorById(int doctorId, DateTime date = default)
        {
            // Проверяем дату, если её не передали, тогда берем текущий день
            if (date == default)
                date = DateTime.Today;

            double load = 0f; // Выставляем базовую нагрузку в 0
            var selectedDate = DateConverter.DateTimeToString(date);

            // Получаем список талонов к текущему доктору
            var tickets = MainWindowVM.DataBase.Tickets.Where((ticket) => ticket.DoctorID == doctorId)?.ToList();

            foreach(var item in new List<Ticket>(tickets)) // Проходим по всем талонам
            {
                var ticketDate = DateConverter.StringToDateTime(item.DateTime);
                if (date.Date != ticketDate.Date) // Если дата талона не совпадает, тогда убираем его из списка
                    tickets.Remove(item);
            }

            foreach (var item in tickets) // Идем по оставшимся талонам
                if (item.UserID != null) // Если талон зарезервирован - увеличиваем нагрузку на 1
                    load++;

            int ticketsCount = tickets.Count();
            var resultLoad = ticketsCount == 0 ? 0 : load / ticketsCount * 100; // Считаем нагрузку путем деления количества зарезервированных талонов на общее количество талонов

            return resultLoad;
        }
        #endregion
    }
}
