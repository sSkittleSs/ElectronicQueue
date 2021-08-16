using ElectronicQueue.ViewModels;
using System.Linq;

namespace ElectronicQueue.DataBase.Models
{
    public class User : BaseVM
    {
        #region Fields
        private string name;
        private string birthday;
        private string username;
        private string password;
        private string email;
        private long phoneNumber;
        private int userID;
        private int roleID;
        #endregion

        #region Properties
        public string Name
        {
            set => SetProperty(ref name, value);
            get => name;
        }

        public string Birthday
        {
            set => SetProperty(ref birthday, value);
            get => birthday;
        }

        public string Username
        {
            set => SetProperty(ref username, value);
            get => username;
        }

        public string Password
        {
            set => SetProperty(ref password, value);
            get => password;
        }

        public string Email
        {
            set => SetProperty(ref email, value);
            get => email;
        }

        public long PhoneNumber
        {
            set => SetProperty(ref phoneNumber, value);
            get => phoneNumber;
        }

        public int UserID
        {
            set => SetProperty(ref userID, value);
            get => userID;
        }

        public int RoleID
        {
            set => SetProperty(ref roleID, value);
            get => roleID;
        }
        #endregion

        #region InnerEnumerations
        public enum UserProperties { Name, Birthday, Username, Password, Email, PhoneNumber, RoleID }
        #endregion

        #region Constructors
        public User() { }

        public User(string name, string birthday, string username, string email, long phoneNumber, string password)
        {
            Name = name;
            Birthday = birthday;
            Username = username;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
        }

        public User(string name, string birthday, string username, string email, long phoneNumber, string password, int roleID) : this(name, birthday, username, email, phoneNumber, password)
        {
            RoleID = roleID;
        }

        public User(string name, string birthday, string username, string email, long phoneNumber, string password, int roleID, int userID) : this(name, birthday, username, email, phoneNumber, password, roleID)
        {
            UserID = userID;
        }
        #endregion

        #region Commands / Methods
        public override string ToString() => $"Name: {Name}\nBirthday: {Birthday}\nUsername: {Username}\nPassword: {Password}\nEmail: {Email}\nPhoneNumber: {PhoneNumber}\nUserID: {UserID}\nRoleID: {RoleID}";

        public static string GetPropertyInStringByUserID(int userID, UserProperties property)
        {
            switch (property)
            {
                case UserProperties.Name:
                    return MainWindowVM.DataBase.Users.FirstOrDefault((user) => user.UserID == userID)?.Name;
                case UserProperties.Birthday:
                    return MainWindowVM.DataBase.Users.FirstOrDefault((user) => user.UserID == userID)?.Birthday;
                case UserProperties.Username:
                    return MainWindowVM.DataBase.Users.FirstOrDefault((user) => user.UserID == userID)?.Username;
                case UserProperties.Password:
                    return MainWindowVM.DataBase.Users.FirstOrDefault((user) => user.UserID == userID)?.Password;
                case UserProperties.Email:
                    return MainWindowVM.DataBase.Users.FirstOrDefault((user) => user.UserID == userID)?.Email;
                case UserProperties.PhoneNumber:
                    return MainWindowVM.DataBase.Users.FirstOrDefault((user) => user.UserID == userID)?.PhoneNumber.ToString();
                case UserProperties.RoleID:
                    return MainWindowVM.DataBase.Users.FirstOrDefault((user) => user.UserID == userID)?.RoleID.ToString();
                default:
                    return "NONE";
            }
        }
        #endregion
    }
}
