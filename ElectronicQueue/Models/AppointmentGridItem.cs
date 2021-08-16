using ElectronicQueue.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicQueue.Models
{
    class AppointmentGridItem : BaseVM
    {
        #region Fields
        private string freeDate;
        private string privatedDate;
        #endregion

        #region Propeties
        public string FreeDate
        {
            set => SetProperty(ref freeDate, value);
            get => freeDate;
        }

        public string PrivatedDate
        {
            set => SetProperty(ref privatedDate, value);
            get => privatedDate;
        }
        #endregion

        #region Constructors
        public AppointmentGridItem() { }

        public AppointmentGridItem(string freeDate, string privatedDate)
        {
            FreeDate = freeDate;
            PrivatedDate = privatedDate;
        }
        #endregion

        #region Methods
        public override string ToString() => $"FreeDate: {FreeDate}\nPrivatedDate: {PrivatedDate}";
        #endregion
    }
}
