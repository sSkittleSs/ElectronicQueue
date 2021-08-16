using ElectronicQueue.ViewModels;
using System.Linq;

namespace ElectronicQueue.DataBase.Models
{
    public class Speciality : BaseVM
    {
        #region Fields
        private string name;
        private int specialityID;
        #endregion

        #region Properties
        public string Name
        {
            set => SetProperty(ref name, value);
            get => name;
        }

        public int SpecialityID
        {
            set => SetProperty(ref specialityID, value);
            get => specialityID;
        }
        #endregion

        #region InnerEnumerations
        public enum SpecialityProperties { Name }
        #endregion

        #region Constructors
        public Speciality() { }

        public Speciality(string name)
        {
            Name = name;
        }

        public Speciality(string name, int specialityID) : this(name)
        {
            SpecialityID = specialityID;
        }
        #endregion

        #region Commands / Methods
        public override string ToString() => $"Name: {Name}\nSpecialtyID: {SpecialityID}";

        public static string GetPropertyInStringBySpecialityID(int specialityID, SpecialityProperties property)
        {
            switch (property)
            {
                case SpecialityProperties.Name:
                    return MainWindowVM.DataBase.Specialities.FirstOrDefault((speciality) => speciality.SpecialityID == specialityID)?.Name;
                default:
                    return "NONE";
            }
        }
        #endregion
    }
}
