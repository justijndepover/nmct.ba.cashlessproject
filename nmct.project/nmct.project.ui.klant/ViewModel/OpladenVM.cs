using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.project.model.dbKlant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.project.ui.klant.ViewModel
{
    class OpladenVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Kaart Opladen"; }
        }

        private int _totalAmount;

        public int TotalAmount
        {
            get { return _totalAmount; }
            set { _totalAmount = value; OnPropertyChanged("TotalAmount"); }
        }

        private string _error;

        public string Error
        {
            get { return _error; }
            set { _error = value; }
        }

        public ICommand AmountCommand
        {
            get { return new RelayCommand<int>(AddValue); }
        }
        
        private void AddValue(int amount)
        {
            TotalAmount += amount;
            if (TotalAmount <= 0)
            {
                TotalAmount = 0;
            }
        }

        public ICommand SaveAmountCommand
        {
            get { return new RelayCommand(PostValue); }
        }

        private async void PostValue()
        {
            double amount = ApplicationVM.ActiveUser.Balance + TotalAmount;
            CustomerTransaction ct = new CustomerTransaction() { RijksregisterNummer = ApplicationVM.ActiveUser.RijksregisterNummer, VerschilBedrag = amount };
            string input = JsonConvert.SerializeObject(ct);

            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                HttpResponseMessage response = await client.PostAsync("http://localhost:3655/api/customer/", new StringContent(input, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                    ApplicationVM.ActiveUser.Balance = amount;
                    appvm.ChangePage(new MainscreenVM());
                }
                else
                {
                    Error = "Transactie mislukt";
                }
            }
        }
    }
}
