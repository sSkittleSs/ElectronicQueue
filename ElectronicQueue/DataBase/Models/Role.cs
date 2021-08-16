using ElectronicQueue.ViewModels;
using System.Linq;

namespace ElectronicQueue.DataBase.Models
{
    public class Role : BaseVM
    {
        #region Fields
        private string name;
        private string description;
        private int roleID;
        #endregion

        #region Properties
        public string Name
        {
            set => SetProperty(ref name, value);
            get => name;
        }

        public string Description
        {
            set => SetProperty(ref description, value);
            get => description;
        }

        public int RoleID
        {
            set => SetProperty(ref roleID, value);
            get => roleID;
        }
        #endregion

        #region InnerEnumerations
        public enum RoleProperties { Name, Description }
        #endregion

        #region Constructors
        public Role() { }

        public Role(string name, string description, int roleID)
        {
            Name = name;
            Description = description;
            RoleID = roleID;
        }
        #endregion

        #region Commands / Methods
        public override string ToString() => $"RoleID: {RoleID}\nName: {Name}\nDescription: {Description}";

        public static string GetPropertyInStringByRoleID(int roleID, RoleProperties property)
        {
            switch (property)
            {
                case RoleProperties.Name:
                    return MainWindowVM.DataBase.Roles.FirstOrDefault((role) => role.RoleID == roleID)?.Name;
                case RoleProperties.Description:
                    return MainWindowVM.DataBase.Roles.FirstOrDefault((role) => role.RoleID == roleID)?.Description;
                default:
                    return "NONE";
            }
        }
        #endregion
    }
}
