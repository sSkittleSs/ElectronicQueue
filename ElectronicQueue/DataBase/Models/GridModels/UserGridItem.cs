using ElectronicQueue.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicQueue.DataBase.Models.GridModels
{
    class UserGridItem : BaseVM
    {
        #region Fields
        private int userId;
        private string name;
        #endregion

        #region Propeties
        public int UserId
        {
            set => SetProperty(ref userId, value);
            get => userId;
        }

        public string Name
        {
            set => SetProperty(ref name, value);
            get => name;
        }
        #endregion

        #region Constructors
        public UserGridItem() { }

        public UserGridItem(int userId, string name)
        {
            UserId = userId;
            Name = name;
        }
        #endregion

        #region Methods
        public override string ToString() => Name;
        #endregion
    }
}
