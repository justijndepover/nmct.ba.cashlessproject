using GalaSoft.MvvmLight.Command;
using nmct.project.model.dbKlant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.project.ui.medewerker.ViewModel
{
    class ApplicationVM : ObservableObject
    {
        public ApplicationVM()
        {
            Token = GetToken();
            Pages.Add(new LoginVM());
            Pages.Add(new MainscreenVM());
            Pages.Add(new TussenpaginaVM());
            // Add other pages

            CurrentPage = Pages[0];
        }

        private static DateTime _from;

        public static DateTime From
        {
            get { return _from; }
            set { _from = value; }
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

        public static TokenResponse Token { get; set; }

        public TokenResponse GetToken()
        {
            OAuth2Client client = new OAuth2Client(new Uri("http://localhost:3655/token"));
            return client.RequestResourceOwnerPasswordAsync(Properties.Settings.Default.DbLogin, Properties.Settings.Default.DbPassword).Result;
        }

        private static Employee _activeEmployee;

        public static Employee ActiveEmployee
        {
            get { return _activeEmployee; }
            set { _activeEmployee = value; }
        }

        private static Customer _activeCustomer;

        public static Customer ActiveCustomer
        {
            get { return _activeCustomer; }
            set { _activeCustomer = value; }
        }
          
    }
}
