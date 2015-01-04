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
    class KassasVM:ObservableObject, IPage
    {
        public KassasVM()
        {
            if (ApplicationVM.token != null)
            {
                GetRegisters();
            }
        }
        public string Name
        {
            get { return "Kassa's"; }
        }

        private ObservableCollection<Registers> _allKassas;

        public ObservableCollection<Registers> AllKassas
        {
            get { return _allKassas; }
            set { _allKassas = value; OnPropertyChanged("AllKassas"); }
        }

        private Registers _selectedKassa;

        public Registers SelectedKassa
        {
            get { return _selectedKassa; }
            set { _selectedKassa = value; OnPropertyChanged("SelectedKassa"); }
        }
        

        public async void GetRegisters()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:3655/api/Register");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    AllKassas = JsonConvert.DeserializeObject<ObservableCollection<Registers>>(json);
                }
            }
        }

        private ObservableCollection<RegisterEmployee> _registerEmployee;

        public ObservableCollection<RegisterEmployee> RegisterEmployee
        {
            get { return _registerEmployee; }
            set { _registerEmployee = value; OnPropertyChanged("RegisterEmployee"); }
        }
        

        public ICommand RegisterDetailCommand
        {
            get { return new RelayCommand<int>(GetRegisterDetails); }
        }

        private async void GetRegisterDetails(int registerID)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:3655/api/register/?id=" + registerID);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    RegisterEmployee = JsonConvert.DeserializeObject<ObservableCollection<RegisterEmployee>>(json);
                }
            }
        }
    }
}
