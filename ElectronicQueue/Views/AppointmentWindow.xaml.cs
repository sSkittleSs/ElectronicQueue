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
    /// Логика взаимодействия для AppointmentWindow.xaml
    /// </summary>
    public partial class AppointmentWindow : UserControl
    {
        public AppointmentWindow()
        {
            InitializeComponent();

            DataContext = new AppointmentWindowVM();
        }

        // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!     Переписать под MVVM (Переписано)     !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //private void special_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var selectedItem = ((ComboBox)sender).SelectedItem.ToString();
        //    ((AppointmentWindowVM)DataContext).SetDoctorsBySpecialityID(MainWindowVM.DataBase.Specialities.FirstOrDefault((spec) => spec.Name == selectedItem)?.SpecialityID ?? 1);
        //}

        //private void docto_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    var selectedName = ((ComboBox)sender).SelectedItem?.ToString();
        //    var userId = MainWindowVM.DataBase.Users.FirstOrDefault((us) => us.Name == selectedName)?.UserID;
        //    var id = MainWindowVM.DataBase.Doctors.FirstOrDefault((doc) => doc.UserID == userId)?.DoctorID ?? 0;

        //    ((AppointmentWindowVM)DataContext).IsQueueAdmin = MainWindowVM.User.UserID == userId;
        //    ((AppointmentWindowVM)DataContext).SetInfoForDoctorByID(id);
        //    ((AppointmentWindowVM)DataContext).SetItemsForDoctorByID(id);
        //}
    }
}
