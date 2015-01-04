using be.belgium.eid;
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

namespace nmct.project.ui.medewerker.ViewModel
{
    class MainscreenVM : ObservableObject, IPage
    {
        public MainscreenVM()
        {
            if (ApplicationVM.Token != null)
            {
                GetProducts();
                SelectedProducts = new ObservableCollection<Products>();
            }
        }
        public string Name
        {
            get { return "Kassa"; }
        }

        private ObservableCollection<Products> _allProducts;

        public ObservableCollection<Products> AllProducts
        {
            get { return _allProducts; }
            set { _allProducts = value; OnPropertyChanged("AllProducts"); }
        }

        private ObservableCollection<Products> _selectedProducts;

        public ObservableCollection<Products> SelectedProducts
        {
            get { return _selectedProducts; }
            set { _selectedProducts = value; OnPropertyChanged("SelectedProducts"); }
        }

        private double _totalCost;

        public double TotalCost
        {
            get { return _totalCost; }
            set { _totalCost = value; OnPropertyChanged("TotalCost"); }
        }

        private string _error;

        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }
        

        private async void GetProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:3655/api/product");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    AllProducts = JsonConvert.DeserializeObject<ObservableCollection<Products>>(json);
                }
            }
        }

        private void CheckBalance()
        {
            if (TotalCost > ApplicationVM.ActiveCustomer.Balance)
            {
                Error = "De rekening is te groot";
            }
            else
            {
                Error = "";
            }
        }

        public ICommand AddProductCommand
        {
            get { return new RelayCommand<int>(AddProduct); }
        }

        public void AddProduct(int ID)
        {
            Products p = AllProducts.Where(x=>x.ID == ID).First();
            TotalCost += p.Price;
            CheckBalance();
            SelectedProducts.Add(p);
        }

        public ICommand ScanCustomerCommand
        {
            get { return new RelayCommand(ScanCustomer); }
        }

        public void ScanCustomer()
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
                    Error = "De gebruiker is niet gevonden.";
                }
            }
        }

        public ICommand DeleteProductCommand
        {
            get { return new RelayCommand<int>(DeleteProduct); }
        }

        public void DeleteProduct(int ID)
        {
            Products p = SelectedProducts.Where(x => x.ID == ID).First();
            TotalCost -= p.Price;
            if (TotalCost < 0)
            {
                TotalCost = 0;
            }
            CheckBalance();
            SelectedProducts.Remove(p);
        }

        public ICommand BestellingCommand
        {
            get { return new RelayCommand(SaveSales); }
        }


        private async void SaveSales()
        {
            ProductList p = new ProductList();
            double verschil = ApplicationVM.ActiveCustomer.Balance - TotalCost;
            p.CustomerTransaction = new CustomerTransaction() { RijksregisterNummer = ApplicationVM.ActiveCustomer.RijksregisterNummer, VerschilBedrag = verschil };
            p.Products = SelectedProducts;
            p.CustomerID = ApplicationVM.ActiveCustomer.ID;
            p.RegisterID = int.Parse(Properties.Settings.Default.RegisterID);
            p.TimeStamp = DateTime.Now;
            string input = JsonConvert.SerializeObject(p);
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                HttpResponseMessage response = await client.PostAsync("http://localhost:3655/api/sales", new StringContent(input, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                    ApplicationVM.ActiveCustomer = new Customer();
                    appvm.ChangePage(new TussenpaginaVM());
                }
                else
                {
                    Error = "Transactie mislukt";
                }
            }
        }

        
    }
}
