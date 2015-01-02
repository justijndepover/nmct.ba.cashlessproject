using Newtonsoft.Json;
using nmct.project.model.dbKlant;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.management.ui.ViewModel
{
    class KlantenVM : ObservableObject, IPage
    {
        public KlantenVM()
        {
            if (ApplicationVM.token != null)
            {
                GetCustomers();   
            }
        }
        public string Name
        {
            get {return "Klanten"; }
        }

        private ObservableCollection<Customer> _allCustomers;

        public ObservableCollection<Customer> AllCustomers
        {
            get { return _allCustomers; }
            set { _allCustomers = value; OnPropertyChanged("AllCustomers"); }
        }

        private async void GetCustomers()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:3655/Customer");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    AllCustomers = JsonConvert.DeserializeObject<ObservableCollection<Customer>>(json);
                }
            }
        }
        
    }
}
