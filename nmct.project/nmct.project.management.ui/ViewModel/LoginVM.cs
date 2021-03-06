﻿using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using nmct.project.model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.project.management.ui.ViewModel
{
    class LoginVM : ObservableObject, IPage
    {
        public LoginVM()
        {
            
        }
        public string Name
        {
            get { return "Login"; }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged("Username"); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("Password"); }
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
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            ApplicationVM.token = GetToken();

            if (!ApplicationVM.token.IsError)
            {
                GetOrganisation();
                appvm.ChangePage(new MainscreenVM());
            }
            else
            {
                Error = "Gebruikersnaam of paswoord kloppen niet";
                ApplicationVM.CurrentOrganisation = null;
            }
        }

        private async void GetOrganisation()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:3655/api/organisation/?username=" + Username);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    ApplicationVM.CurrentOrganisation = JsonConvert.DeserializeObject<Organisations>(json);
                }
            }
        }

        private TokenResponse GetToken()
        {
            OAuth2Client client = new OAuth2Client(new Uri("http://localhost:3655/token"));
            return client.RequestResourceOwnerPasswordAsync(Username, Password).Result;
        }
    }
}
