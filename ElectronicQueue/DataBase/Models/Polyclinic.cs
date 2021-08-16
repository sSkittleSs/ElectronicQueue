using ElectronicQueue.ViewModels;
using System.Linq;

namespace ElectronicQueue.DataBase.Models
{
    public class Polyclinic : BaseVM
    {
        #region Fields
        private int number;
        private int adressID;
        private int polyclinicID;
        #endregion

        #region Properties
        public int Number
        {
            set => SetProperty(ref number, value);
            get => number;
        }

        public int AdressID
        {
            set => SetProperty(ref adressID, value);
            get => adressID;
        }

        public int PolyclinicID
        {
            set => SetProperty(ref polyclinicID, value);
            get => polyclinicID;
        }
        #endregion

        #region InnerEnumerations
        public enum PolyclinicProperties { Number, AdressID }
        #endregion

        #region Constructors
        public Polyclinic() { }

        public Polyclinic(int number, int adressID)
        {
            Number = number;
            AdressID = adressID;
        }

        public Polyclinic(int number, int adressID, int PolyclinicID) : this(number, adressID)
        {
            PolyclinicID = polyclinicID;
        }
        #endregion

        #region Commands / Methods
        public override string ToString() => $"Number: {Number}\nAdressID: {AdressID}\nPolyclinicID: {PolyclinicID}";

        public static int GetPropertyByPolyclinicID(int polyclinicID, PolyclinicProperties property)
        {
            switch (property)
            {
                case PolyclinicProperties.Number:
                    return MainWindowVM.DataBase.Polyclinics.FirstOrDefault((polyclinic) => polyclinic.PolyclinicID == polyclinicID)?.Number ?? -1;
                case PolyclinicProperties.AdressID:
                    return MainWindowVM.DataBase.Polyclinics.FirstOrDefault((polyclinic) => polyclinic.PolyclinicID == polyclinicID)?.AdressID ?? -1;
                default:
                    return -1;
            }
        }
        #endregion
    }
}
