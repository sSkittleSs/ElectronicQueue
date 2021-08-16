using ElectronicQueue.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicQueue.DataBase.Models.GridModels
{
    class SpecialityGridItem : BaseVM
    {
        #region Fields
        private int specialityId;
        private string name;
        #endregion

        #region Propeties
        public int SpecialityId
        {
            set => SetProperty(ref specialityId, value);
            get => specialityId;
        }

        public string Name
        {
            set => SetProperty(ref name, value);
            get => name;
        }
        #endregion

        #region Constructors
        public SpecialityGridItem() { }

        public SpecialityGridItem(int specialityId, string name)
        {
            SpecialityId = specialityId;
            Name = name;
        }
        #endregion

        #region Methods
        public override string ToString() => Name;
        #endregion
    }
}
