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
    }
}
