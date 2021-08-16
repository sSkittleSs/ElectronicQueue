using ElectronicQueue.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicQueue.DataBase.Models.GridModels
{
    class DoctorGridItem : BaseVM
    {
        #region Fields
        private int doctorId;
        private string name;
        #endregion

        #region Propeties
        public int DoctorId
        {
            set => SetProperty(ref doctorId, value);
            get => doctorId;
        }

        public string Name
        {
            set => SetProperty(ref name, value);
            get => name;
        }
        #endregion

        #region Constructors
        public DoctorGridItem() { }

        public DoctorGridItem(int doctorId, string name)
        {
            DoctorId = doctorId;
            Name = name;
        }
        #endregion

        #region Methods
        public override string ToString() => Name;
        #endregion
    }
}
