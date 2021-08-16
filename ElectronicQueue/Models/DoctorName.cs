using ElectronicQueue.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ElectronicQueue.Models
{
    class DoctorName : BaseVM, IComparable
    {
        #region Fields
        private string name;
        private string load;
        private Brush loadColor;
        #endregion

        #region Propeties
        public string Name
        {
            set => SetProperty(ref name, value);
            get => name;
        }

        public string Load
        {
            set => SetProperty(ref load, value);
            get => load;
        }

        public Brush LoadColor
        {
            set => SetProperty(ref loadColor, value);
            get => loadColor;
        }
        #endregion

        #region Constructors
        public DoctorName(string name, string load = "0", Brush loadColor = default)
        {
            Name = name;
            Load = load;
            LoadColor = loadColor == default ? Brushes.Gray : loadColor;
        }
        #endregion

        #region Methods
        public override string ToString() => Name;

        public int CompareTo(object obj) => Name.CompareTo(obj);
        #endregion
    }
}
