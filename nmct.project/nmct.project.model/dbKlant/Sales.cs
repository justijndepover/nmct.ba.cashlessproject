using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.model.dbKlant
{
    public class Sales
    {
        #region "properties"

        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _timestamp;
        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
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

        private int _productID;        
        public int ProductID
        {
            get { return _productID; }
            set { _productID = value; }
        }

        private double _amount;
        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        private double _totalPrice;
        public double TotalPrice
        {
            get { return _totalPrice; }
            set { _totalPrice = value; }
        }
        
        #endregion
    }
}
