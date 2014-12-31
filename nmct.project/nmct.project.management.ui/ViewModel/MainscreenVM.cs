using Newtonsoft.Json;
using nmct.project.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.management.ui.ViewModel
{
    class MainscreenVM: ObservableObject, IPage
    {
        public MainscreenVM()
        {
            if (ApplicationVM.token != null)
            {
                GetOrganisationName();
            }
        }

        public async void GetOrganisationName()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:3655/api/organisation/?username=" + ApplicationVM.CurrentUser);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    OrganisationName = JsonConvert.DeserializeObject<string>(json);
                }
            }
        }

        public string Name
        {
            get { return "Mainscreen"; }
        }

        private string _organisationName;

        public string OrganisationName
        {
            get { return _organisationName; }
            set { _organisationName = value; OnPropertyChanged("OrganisationName"); }
        }
        
    }
}
