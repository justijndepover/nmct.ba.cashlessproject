using be.belgium.eid;
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

namespace nmct.project.ui.medewerker.ViewModel
{
    class TussenpaginaVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Log klant in"; }
        }

        private string _error;

        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }
        

        public ICommand LoginCommand
        {
            get { return new RelayCommand(Login); }
        }

        private void Login()
        {
            BEID_ReaderSet.initSDK();
            try
            {
                if (BEID_ReaderSet.instance().readerCount() > 0)
                {
                    BEID_ReaderContext readerContext = readerContext = BEID_ReaderSet.instance().getReader();
                    if (readerContext != null)
                    {
                        if (readerContext.getCardType() == BEID_CardType.BEID_CARDTYPE_EID)
                        {
                            Customer c = new Customer();
                            BEID_EIDCard card = readerContext.getEIDCard();
                            c.RijksregisterNummer = long.Parse(card.getID().getNationalNumber());
                            ControleerGebruiker(c);
                        }
                    }
                }
                else
                {
                    Error = "Gelieve een geregistreerde klanten ID in de reader te plaatsen";
                }
                BEID_ReaderSet.releaseSDK();
            }
            catch (Exception ex)
            {
                Error = "Gelieve een geregistreerde klanten ID in de reader te plaatsen";
            }
        }

        private async void ControleerGebruiker(Customer c)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:3655/Api/Customer/?nummer=" + c.RijksregisterNummer);
                string json = await response.Content.ReadAsStringAsync();
                if (json != "null")
                {
                    ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                    ApplicationVM.ActiveCustomer = JsonConvert.DeserializeObject<Customer>(json);

                    appvm.ChangePage(new MainscreenVM());
                }
                else
                {
                    Error = "Gelieve een geregistreerde klanten ID in de reader te plaatsen";
                }
            }
        }

        public ICommand UitloggenCommand
        {
            get { return new RelayCommand(MedewerkerLoggen); }
        }

        private async void MedewerkerLoggen()
        {
            RegisterEmployee re = new RegisterEmployee() { RegisterID = int.Parse(Properties.Settings.Default.RegisterID), EmployeeID = ApplicationVM.ActiveEmployee.ID, From = ApplicationVM.From, Until = DateTime.Now };
            string input = JsonConvert.SerializeObject(re);
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                HttpResponseMessage response = await client.PostAsync("http://localhost:3655/api/register/", new StringContent(input, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                    appvm.ChangePage(new LoginVM());
                }
            }
        }
    }
}
