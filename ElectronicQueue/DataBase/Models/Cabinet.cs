using ElectronicQueue.ViewModels;
using System.Linq;

namespace ElectronicQueue.DataBase.Models
{
    public class Cabinet : BaseVM
    {
        #region Fields
        private int number;
        private int polyclinicID;
        private int cabinetID;
        #endregion

        #region Properties
        public int Number
        {
            set => SetProperty(ref number, value);
            get => number;
        }

        public int PolyclinicID
        {
            set => SetProperty(ref polyclinicID, value);
            get => polyclinicID;
        }

        public int CabinetID
        {
            set => SetProperty(ref cabinetID, value);
            get => cabinetID;
        }
        #endregion

        #region InnerEnumerations
        public enum CabinetProperties { Number, PolyclinicID }
        #endregion

        #region Constructors
        public Cabinet() { }

        public Cabinet(int number, int polyclinicID)
        {
            Number = number;
            PolyclinicID = polyclinicID;
        }

        public Cabinet(int number, int polyclinicID, int cabinetID) : this(number, polyclinicID)
        {
            CabinetID = cabinetID;
        }
        #endregion

        #region Commands / Methods
        public override string ToString() => $"Number: {Number}\nPolyclinicID: {PolyclinicID}\nCabinetID: {CabinetID}";

        public static int GetPropertyByCabinetID(int cabinetID, CabinetProperties property)
        {
            switch (property)
            {
                case CabinetProperties.Number:
                    return MainWindowVM.DataBase.Cabinets.FirstOrDefault((cabinet) => cabinet.CabinetID == cabinetID)?.Number ?? -1;
                case CabinetProperties.PolyclinicID:
                    return MainWindowVM.DataBase.Cabinets.FirstOrDefault((cabinet) => cabinet.CabinetID == cabinetID)?.PolyclinicID ?? -1;
                default:
                    return -1;
            }
        }
        #endregion
    }
}
