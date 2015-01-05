using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.project.model.dbKlant;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

        private Customer _selectedCustomer;

        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set { _selectedCustomer = value; OnPropertyChanged("SelectedCustomer"); }
        }
        

        private async void GetCustomers()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:3655/api/Customer");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    AllCustomers = JsonConvert.DeserializeObject<ObservableCollection<Customer>>(json);
                }
            }            
        }

        public ICommand CustomerDetailCommand
        {
            get { return new RelayCommand<int>(ShowDetail); }
        }

        private void ShowDetail(int id)
        {
            SelectedCustomer = (Customer)AllCustomers.Where(c => c.ID == id).FirstOrDefault();
        }

        public ICommand SaveCustomerCommand
        {
            get { return new RelayCommand(Save); }
        }

        private async void Save()
        {
            string input = JsonConvert.SerializeObject(SelectedCustomer);
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.PostAsync("http://localhost:3655/api/customer/", new StringContent(input, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    GetCustomers();
                }
            }
        }
    }
}
