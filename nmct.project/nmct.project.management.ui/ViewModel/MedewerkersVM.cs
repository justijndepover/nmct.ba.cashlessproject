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
    class MedewerkersVM : ObservableObject, IPage
    {

        public string Name
        {
            get { return "Medewerkers"; }
        }

        private ObservableCollection<Employee> _allEmployees;

        public ObservableCollection<Employee> AllEmployees
        {
            get { return _allEmployees; }
            set { _allEmployees = value; OnPropertyChanged("AllEmployees"); }
        }

        public async void GetEmployees()
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("http://localhost:3655/api/employee");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    AllEmployees = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
                }
            }
        }
    }
}
