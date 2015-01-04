using GalaSoft.MvvmLight.Command;
using nmct.project.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.project.management.ui.ViewModel
{
    class ApplicationVM : ObservableObject
    {
        public ApplicationVM()
        {
            Pages.Add(new LoginVM());
            Pages.Add(new MainscreenVM());
            Pages.Add(new ProductenVM());
            Pages.Add(new MedewerkersVM());
            Pages.Add(new KassasVM());
            Pages.Add(new KlantenVM());
            Pages.Add(new SettingsVM());
            // Add other pages

            CurrentPage = Pages[0];
        }

        public static TokenResponse token = null;

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

        private static Organisations _currentOrganisation;

        public static Organisations CurrentOrganisation
        {
            get { return _currentOrganisation; }
            set { _currentOrganisation = value; }
        }
              
    }
}
