using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace nmct.project.model.dbKlant
{
    public class Products : INotifyPropertyChanged
    {
        public Products()
        {
            Edit = Visibility.Collapsed;
        }
        #region "properties"

        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _productName;
        public string ProductName
        {
            get { return _productName; }
            set { _productName = value; }
        }

        private double _price;
        public double Price
        {
            get { return _price; }
            set { _price = value; }
        }

        private Visibility _edit;
        public Visibility Edit
        {
            get { return _edit; }
            set { _edit = value; OnPropertyChanged("Edit"); }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
