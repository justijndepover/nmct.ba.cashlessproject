using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.model.dbKlant
{
    public class ProductList
    {
        private ObservableCollection<Products> _products;

        public ObservableCollection<Products> Products
        {
            get { return _products; }
            set { _products = value; }
        }

        private CustomerTransaction _customerTransaction;

        public CustomerTransaction CustomerTransaction
        {
            get { return _customerTransaction; }
            set { _customerTransaction = value; }
        }

        private int _customerID;

        public int CustomerID
        {
            get { return _customerID; }
            set { _customerID = value; }
        }

        private int _registerID;

        public int RegisterID
        {
            get { return _registerID; }
            set { _registerID = value; }
        }

        private DateTime _timeStamp;

        public DateTime TimeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }
          
    }
}
