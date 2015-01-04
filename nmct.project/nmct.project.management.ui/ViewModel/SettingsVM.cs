using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using nmct.project.management.ui.Helper;
using nmct.project.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.project.management.ui.ViewModel
{
    class SettingsVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Instellingen"; }
        }

        private string _username;

        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged("Username"); }
        }

        private string _oldPassword;

        public string OldPassword
        {
            get { return _oldPassword; }
            set { _oldPassword = value; OnPropertyChanged("OldPassword"); }
        }

        private string _newPassword;

        public string NewPassword
        {
            get { return _newPassword; }
            set { _newPassword = value; OnPropertyChanged("NewPassword"); }
        }

        private string _error;

        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }
        

        public ICommand UpdatePasswordCommand
        {
            get { return new RelayCommand(UpdatePassword); }
        }

        private async void UpdatePassword()
        {
            if (Cryptography.Encrypt(Username)==ApplicationVM.CurrentOrganisation.Login && Cryptography.Encrypt(OldPassword)==ApplicationVM.CurrentOrganisation.Password)
            {
                Organisations o = ApplicationVM.CurrentOrganisation;
                o.Password = Cryptography.Encrypt(NewPassword);
                string input = JsonConvert.SerializeObject(o);
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.PostAsync("http://localhost:3655/api/organisation/", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
                        appvm.ChangePage(new MainscreenVM());
                    }
                }
            }
            else
            {
                Error = "Gebruikersnaam of wachtwoord klopt niet";
            }
        }
    }
}
