using ElectronicQueue.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ElectronicQueue.DataBase.Models
{
    public class Adres : BaseVM
    {
        #region Fields
        private string adress;
        private int adressID;
        #endregion

        #region Properties
        public string Adress
        {
            set => SetProperty(ref adress, value);
            get => adress;
        }

        [Key]
        public int AdressID
        {
            set => SetProperty(ref adressID, value);
            get => adressID;
        }
        #endregion

        #region InnerEnumerations
        public enum AdresProperties { Adress }
        #endregion

        #region Constructors
        public Adres() { }

        public Adres(string adress)
        {
            Adress = adress;
        }

        public Adres(string adress, int adressID) : this(adress)
        {
            AdressID = adressID;
        }
        #endregion

        #region Commands / Methods
        public override string ToString() => $"Adress: {Adress}\nAdressID: {AdressID}";

        public static string GetPropertyInStringByAdressID(int adressID, AdresProperties property)
        {
            switch (property)
            {
                case AdresProperties.Adress:
                    return MainWindowVM.DataBase.Adreses.FirstOrDefault((adress) => adress.AdressID == adressID).Adress;
                default:
                    return "NONE";
            }
        }
        #endregion
    }
}
