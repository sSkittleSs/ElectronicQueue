using ElectronicQueue.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicQueue.Models
{
    public class UCPGridItem : BaseVM
    {
        #region Fields
        private string date;
        private string speciality;
        #endregion

        #region Properties
        public string Date
        {
            set => SetProperty(ref date, value);
            get => date;
        }

        public string Speciality
        {
            set => SetProperty(ref speciality, value);
            get => speciality;
        }
        #endregion

        #region Contructors
        public UCPGridItem(string date, string speciality)
        {
            Date = date;
            Speciality = speciality;
        }
        #endregion
    }
}
