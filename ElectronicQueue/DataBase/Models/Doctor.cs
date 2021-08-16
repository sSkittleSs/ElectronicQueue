using ElectronicQueue.ViewModels;
using System.Linq;

namespace ElectronicQueue.DataBase.Models
{
    public class Doctor : BaseVM
    {
        #region Fields
        private int userID;
        private int specialityID;
        private int cabinetID;
        private int doctorID;
        #endregion

        #region Properties
        public int UserID
        {
            set => SetProperty(ref userID, value);
            get => userID;
        }

        public int SpecialityID
        {
            set => SetProperty(ref specialityID, value);
            get => specialityID;
        }

        public int CabinetID
        {
            set => SetProperty(ref cabinetID, value);
            get => cabinetID;
        }

        public int DoctorID
        {
            set => SetProperty(ref doctorID, value);
            get => doctorID;
        }
        #endregion

        #region InnerEnumerations
        public enum DoctorProperties { UserID, SpecialityID, CabinetID }
        #endregion

        #region Constructors
        public Doctor() { }

        public Doctor(int userID, int specialityID, int cabinetID)
        {
            UserID = userID;
            SpecialityID = specialityID;
            CabinetID = cabinetID;
        }

        public Doctor(int userID, int specialityID, int cabinetID, int doctorID) : this(userID, specialityID, cabinetID)
        {
            DoctorID = doctorID;
        }
        #endregion

        #region Commands / Methods
        public override string ToString() => $"UserID: {UserID}\nSpecialityID: {SpecialityID}\nCabinetID: {CabinetID}\nDoctorID: {DoctorID}";

        public static int GetPropertyByDoctorID(int doctorID, DoctorProperties property)
        {
            switch (property)
            {
                case DoctorProperties.UserID:
                    return MainWindowVM.DataBase.Doctors.FirstOrDefault((doctor) => doctor.DoctorID == doctorID)?.UserID ?? -1;
                case DoctorProperties.SpecialityID:
                    return MainWindowVM.DataBase.Doctors.FirstOrDefault((doctor) => doctor.DoctorID == doctorID)?.SpecialityID ?? -1;
                case DoctorProperties.CabinetID:
                    return MainWindowVM.DataBase.Doctors.FirstOrDefault((doctor) => doctor.DoctorID == doctorID)?.CabinetID ?? -1;
                default:
                    return -1;
            }
        }
        #endregion
    }
}
