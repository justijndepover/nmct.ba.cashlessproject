using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using nmct.project.model.dbKlant;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace nmct.project.management.ui.ViewModel
{
    class ProductenVM: ObservableObject, IPage
    {
        public ProductenVM()
        {
            if (ApplicationVM.token != null)
            {
                GetProducts();
            }
        }
        
        public string Name
        {
            get { return "Producten"; }
        }

        private ObservableCollection<Products> _allProducts;
        public ObservableCollection<Products> AllProducts
        {
            get { return _allProducts; }
            set { _allProducts = value; OnPropertyChanged("AllProducts"); }
        }

        public async void GetProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:3655/api/product");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    AllProducts = JsonConvert.DeserializeObject<ObservableCollection<Products>>(json);
                }
            }
        }

        public ICommand DeleteCommand
        {
            get { return new RelayCommand<int>(DeleteProduct); }
        }

        public async void DeleteProduct(int Id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.DeleteAsync("http://localhost:3655/api/product/"+Id);
            }
            GetProducts();
        }

        public ICommand EditCommand
        {
            get { return new RelayCommand<Products>(EditProduct); }
        }

        public void EditProduct(Products p)
        {
            if (p.Edit == Visibility.Visible)
            {
                p.Edit = Visibility.Collapsed;
            }
            else
            {
                p.Edit = Visibility.Visible;
            }
           
        }

        public ICommand UpdateCommand
        {
            get { return new RelayCommand<Products>(UpdateProduct); }
        }

        public async static void UpdateProduct(Products p)
        {
            using (HttpClient client = new HttpClient())
            {
                //HttpResponseMessage response = await client.PutAsync("http://localhost:3655/api/product/", new HttpContent());
            }
        }

    }
}
