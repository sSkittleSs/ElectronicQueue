using ElectronicQueue.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicQueue.DataBase.Models.GridModels
{
    class CabinetGridItem : BaseVM
    {
        #region Fields
        private int cabinetId;
        private string name;
        #endregion

        #region Propeties
        public int CabinetId
        {
            set => SetProperty(ref cabinetId, value);
            get => cabinetId;
        }

        public string Name
        {
            set => SetProperty(ref name, value);
            get => name;
        }
        #endregion

        #region Constructors
        public CabinetGridItem() { }

        public CabinetGridItem(int cabinetId, string name)
        {
            CabinetId = cabinetId;
            Name = name;
        }
        #endregion

        #region Methods
        public override string ToString() => Name;
        #endregion
    }
}
