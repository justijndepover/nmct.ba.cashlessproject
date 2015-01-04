using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.project.ui.klant.ViewModel
{
    class RegistrerenVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Registreren"; }
        }

        public ICommand RegisterCommand
        {
            get { return new RelayCommand(Register); }
        }

        private async void Register()
        {
            string input = JsonConvert.SerializeObject(ApplicationVM.TempUser);
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.Token.AccessToken);
                HttpResponseMessage response = await client.PutAsync("http://localhost:3655/api/Customer/", new StringContent(input, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("error");
                }
                else
                {
                    ApplicationVM.ActiveUser = ApplicationVM.TempUser;
                    ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                    appvm.ChangePage(new MainscreenVM());
                }
            }
        }
    }

}
