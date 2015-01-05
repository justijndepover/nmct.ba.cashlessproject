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
                            BEID_Picture picture;
                            picture = card.getPicture();
                            byte[] bytearray = picture.getData().GetBytes();
                            c.CustomerName = card.getID().getFirstName() + " " + card.getID().getSurname();
                            c.Address = card.getID().getStreet() + " " + card.getID().getZipCode() + " " + card.getID().getMunicipality();
                            c.Picture = bytearray;
                            c.RijksregisterNummer = long.Parse(card.getID().getNationalNumber());
                            c.Balance = 0;
                            ApplicationVM.TempUser = c;
                            ControleerGebruiker(c);
                        }
                    }
                }
                else
                {
                    Error = "Plaats een geldige kaart in het toestel";
                    CreateErrorlog(Error, "LoginVM", "Login");
                }
                BEID_ReaderSet.releaseSDK();
            }
            catch (Exception ex)
            {
                Error = "Plaats een geldige kaart in het toestel";
                CreateErrorlog(Error, "LoginVM", "Login");
            }
        }


        private async void ControleerGebruiker(Customer c)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:3655/Api/Customer/?nummer=" + c.RijksregisterNummer);
                string json = await response.Content.ReadAsStringAsync();
                ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                if (json != "null")
                {
                    
                    ApplicationVM.ActiveUser = JsonConvert.DeserializeObject<Customer>(json);

                    appvm.ChangePage(new MainscreenVM());
                }
                else
                {
                    appvm.ChangePage(new RegistrerenVM());
                }
            }
        }
        private async void CreateErrorlog(string errorMessage, string errorClass, string errorMethod)
        {
            Errorlog errorlog = new Errorlog();
            errorlog.Message = errorMessage;
            errorlog.Stacktrace = "KLANT: ErrorClass:" + errorClass + " ErrorMethod: " + errorMethod;
            errorlog.Timestamp = DateTime.Now;
            using (HttpClient client = new HttpClient())
            {
                string input = JsonConvert.SerializeObject(errorlog);
                HttpResponseMessage response = await client.PostAsync("http://localhost:3655/api/error", new StringContent(input, Encoding.UTF8, "application/json"));
            }
        }
    }
}
