using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace nmct.project.model.dbKlant
{
    public class Customer
    {
        #region "properties"

        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _customerName;
        public string CustomerName
        {
            get { return _customerName; }
            set { _customerName = value; }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }

        private BitmapImage _picture;
        public BitmapImage Picture
        {
            get { return _picture; }
            set { _picture = value; }
        }

        private double _balance;
        public double Balance
        {
            get { return _balance; }
            set { _balance = value; }
        }

        #endregion
    }
}
