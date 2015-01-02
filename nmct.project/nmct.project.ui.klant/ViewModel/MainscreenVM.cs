using nmct.project.model.dbKlant;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.ui.klant.ViewModel
{
    class MainscreenVM: ObservableObject, IPage
    {
        public string Name
        {
            get { return ApplicationVM.ActiveUser.CustomerName; }
        }

        private ObservableCollection<Sales> _transactions;

        public ObservableCollection<Sales> Transactions
        {
            get { return _transactions; }
            set { _transactions = value; }
        }
        
    }
}
