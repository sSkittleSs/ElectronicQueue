using ElectronicQueue.DataBase;
using ElectronicQueue.Models;
using ElectronicQueue.Views;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicQueue.ViewModels
{
    class MainWindowVM : BaseVM
    {
        #region Fields
        private static int windowWidth = 800;
        private static int windowHeight = 460;
        private static int maxControlWidth = 796;
        private static int maxControlHeight = 380;
        private static MainWindow mainWindow;
        private UserControl currentView;
        private bool isConnected = false;
        private bool isAdmin = false;
        private string selectedItem = "1";
        private static ApplicationContext dataBase;
        private static DataBase.Models.User user;
        #endregion

        #region Properties
        public int MinWidth { get; set; } = 800;
        public int MinHeight { get; set; } = 460;

        public int WindowWidth
        {
            set
            {
                SetProperty(ref windowWidth, value);
                MaxControlWidth = value;
            }
            get => windowWidth;
        }

        public int WindowHeight
        {
            set
            {
                SetProperty(ref windowHeight, value);
                MaxControlHeight = value;
            }
            get => windowHeight;
        }

        public int MaxControlWidth
        {
            set => SetProperty(ref maxControlWidth, value - 4);
            get => maxControlWidth;
        }

        public int MaxControlHeight
        {
            set => SetProperty(ref maxControlHeight, value - 80);
            get => maxControlHeight;
        }

        public static MainWindow MainWindow
        {
            set => mainWindow = value;
            get => mainWindow;
        }

        public UserControl CurrentView
        {
            set => SetProperty(ref currentView, value);
            get => currentView;
        }

        public bool IsConnected
        {
            set => SetProperty(ref isConnected, value);
            get => isConnected;
        }

        public bool IsAdmin
        {
            set => SetProperty(ref isAdmin, value);
            get => isAdmin;
        }

        public string SelectedItem
        {
            set => SetProperty(ref selectedItem, value);
            get => selectedItem;
        }

        public static ApplicationContext DataBase
        {
            set => dataBase = value;
            get => dataBase;
        }

        public static DataBase.Models.User User
        {
            set => user = value;
            get => user;
        }
        #endregion

        #region Constructors
        public MainWindowVM(MainWindow mainWindow)
        {
            MainWindow = mainWindow;

            OpenAuthorize.Execute();
            
            DataBase = new ApplicationContext();
        }
        #endregion

        #region Commands / Methods
        public DelegateCommand Logout => new DelegateCommand(() =>
        {
            User = null;
            IsConnected = false;
            IsAdmin = false;
            DataBase = new ApplicationContext();
            OpenAuthorize.Execute();
        });

        public DelegateCommand CloseApplication => new DelegateCommand(() => Application.Current.Shutdown());

        public DelegateCommand MinimizeApplication => new DelegateCommand(() => Application.Current.MainWindow.WindowState = WindowState.Minimized);

        public DelegateCommand Help => new DelegateCommand(() =>
        {
            if (File.Exists(Directory.GetCurrentDirectory() + "\\Справка\\index.htm"))
            {
                Process.Start(Directory.GetCurrentDirectory() + "\\Справка\\index.htm");
                NotificationManager.ShowInformation("Справочная система открывается, ожидайте...");
            }
            else
            {
                NotificationManager.ShowError("Невозможно запустить справочную систему, поскольку она отсутствует или повреждена.");
            }
        });

        #region ItemMenuCommands
        public DelegateCommand OpenAuthorize => new DelegateCommand(() =>
        {
            ChangeView(new AuthorizeWindow());
            SelectedItem = "1";
        });

        public DelegateCommand OpenRegistation => new DelegateCommand(() =>
        {
            ChangeView(new RegistrationWindow());
            SelectedItem = "2";
        });

        public DelegateCommand OpenAppointment => new DelegateCommand(() =>
        {
            ChangeView(new AppointmentWindow());
            SelectedItem = "1";
        });

        public DelegateCommand OpenUCP => new DelegateCommand(() =>
        {
            ChangeView(new UCPWindow());
            SelectedItem = "2";
        });

        public DelegateCommand OpenAdminPanel => new DelegateCommand(() =>
        {
            ChangeView(new AdminPanelWindow());
            SelectedItem = "3";
        });
        #endregion
        public void ChangeView(UserControl userControl) => CurrentView = userControl;

        public void Authorize()
        {
            IsConnected = true;
            OpenUCP.Execute();
        }
        #endregion
    }
}
