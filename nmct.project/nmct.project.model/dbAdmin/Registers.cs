using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.model
{
    public class Registers
    {
        #region "properties"

        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _registerName;
        [Required]
        [DisplayName("Kassanaam")]
        public string RegisterName
        {
            get { return _registerName; }
            set { _registerName = value; }
        }

        private string _device;
        [Required]
        [DisplayName("Toestel")]
        public string Device
        {
            get { return _device; }
            set { _device = value; }
        }

        
        private DateTime _purchaseDate;
        [Required]
        [DisplayName("Aankoopdatum")]
        [DataType(DataType.Date)]
        public DateTime PurchaseDate
        {
            get { return _purchaseDate; }
            set { _purchaseDate = value; }
        }


        private DateTime _expiresDate;
        [Required]
        [DisplayName("Vervaldatum")]
        [DataType(DataType.Date)]
        public DateTime ExpiresDate
        {
            get { return _expiresDate; }
            set { _expiresDate = value; }
        }

        #endregion
    }
}
