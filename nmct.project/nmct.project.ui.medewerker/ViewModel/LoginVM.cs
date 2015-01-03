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
    class LoginVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Login"; }
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
                            Employee e = new Employee();
                            BEID_EIDCard card = readerContext.getEIDCard();
                            e.RijksregisterNummer = long.Parse(card.getID().getNationalNumber());
                            ControleerGebruiker(e);
                        }
                    }
                }
                else
                {
                    Error = "Plaats een geldige kaart in het toestel";
                }
                BEID_ReaderSet.releaseSDK();
            }
            catch (Exception ex)
            {
                Error = "Plaats een geldige kaart in het toestel";
            }
        }

        private async void ControleerGebruiker(Employee e)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:3655/Api/Employee/?nummer=" + e.RijksregisterNummer);
                string json = await response.Content.ReadAsStringAsync();
                if (json != "null")
                {
                    ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                    ApplicationVM.ActiveEmployee = JsonConvert.DeserializeObject<Employee>(json);

                    appvm.ChangePage(new TussenpaginaVM());
                }
                else
                {
                    Error = "De gebruiker is niet gevonden.";
                }
            }
        }
    }
}
