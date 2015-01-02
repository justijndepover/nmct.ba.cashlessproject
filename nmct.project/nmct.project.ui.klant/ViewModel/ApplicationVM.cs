using GalaSoft.MvvmLight.Command;
using nmct.project.model.dbKlant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.project.ui.klant.ViewModel
{
    class ApplicationVM : ObservableObject
    {
        public ApplicationVM()
        {
            Token = GetToken();
            Pages.Add(new LoginVM());
            Pages.Add(new RegistrerenVM());
            Pages.Add(new MainscreenVM());

            CurrentPage = Pages[0];
        }

        private object currentPage;
        public object CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; OnPropertyChanged("CurrentPage"); }
        }

        private List<IPage> pages;
        public List<IPage> Pages
        {
            get
            {
                if (pages == null)
                    pages = new List<IPage>();
                return pages;
            }
        }

        public ICommand ChangePageCommand
        {
            get { return new RelayCommand<IPage>(ChangePage); }
        }

        public void ChangePage(IPage page)
        {
            CurrentPage = page;
        }

        private string _organisation;

        public string Organisation
        {
            get { return _organisation; }
            set { _organisation = value; }
        }

        private static Customer _activeUser;

        public static Customer ActiveUser
        {
            get { return _activeUser; }
            set { _activeUser = value; }
        }

        public static TokenResponse Token { get; set; }

        public TokenResponse GetToken()
        {
            OAuth2Client client = new OAuth2Client(new Uri("http://localhost:3655/token"));
            return client.RequestResourceOwnerPasswordAsync(Properties.Settings.Default.DbLogin, Properties.Settings.Default.DbPassword).Result;
        }
    }
}
