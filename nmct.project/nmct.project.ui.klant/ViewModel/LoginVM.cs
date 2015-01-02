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

namespace nmct.project.ui.klant.ViewModel
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
		    get { return _error;}
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
                    Error = "Plaats een geldige kaart in het toestel";
                }
                BEID_ReaderSet.releaseSDK();
            }
            catch (Exception ex)
            {
                Error = "Plaats een geldige kaart in het toestel";
            }
        }

        public ICommand RegisterCommand
        {
            get { return new RelayCommand(Register); }
        }

        private void Register()
        {
            BEID_ReaderSet.initSDK();
            if (BEID_ReaderSet.instance().readerCount() > 0)
            {
                BEID_ReaderContext readerContext = readerContext = BEID_ReaderSet.instance().getReader();
                if (readerContext != null)
                {
                    if (readerContext.getCardType() == BEID_CardType.BEID_CARDTYPE_EID)
                    {
                        BEID_EIDCard card = readerContext.getEIDCard();
                        BEID_Picture picture;
                        picture = card.getPicture();
                        byte[] bytearray = picture.getData().GetBytes();
                        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                        Customer c = new Customer();
                        ApplicationVM.ActiveUser = c;
                        ApplicationVM.ActiveUser.CustomerName = card.getID().getFirstName() + " " + card.getID().getSurname();
                        ApplicationVM.ActiveUser.Address = card.getID().getStreet() + " " + card.getID().getZipCode() + " " + card.getID().getMunicipality();
                        ApplicationVM.ActiveUser.Picture = bytearray;
                        ApplicationVM.ActiveUser.RijksregisterNummer = long.Parse(card.getID().getNationalNumber());
                        ApplicationVM.ActiveUser.Balance = 0;

                        appvm.ChangePage(new RegistrerenVM());
                    }
                }
            }
            BEID_ReaderSet.releaseSDK();
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
                    ApplicationVM.ActiveUser = JsonConvert.DeserializeObject<Customer>(json);

                    appvm.ChangePage(new MainscreenVM());
                }
                else
                {
                    Error = "De gebruiker is niet gevonden.";
                }
            }
        }
    }
}
