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
using System.Windows;
using System.Windows.Input;

namespace nmct.project.management.ui.ViewModel
{
    class MedewerkersVM : ObservableObject, IPage
    {
        public MedewerkersVM()
        {
            if (ApplicationVM.token != null)
            {
                GetEmployees();
            }
        }
        public string Name
        {
            get { return "Medewerkers"; }
        }

        private ObservableCollection<Employee> _allEmployees;

        public ObservableCollection<Employee> AllEmployees
        {
            get { return _allEmployees; }
            set { _allEmployees = value; OnPropertyChanged("AllEmployees"); }
        }

        public async void GetEmployees()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:3655/api/employee");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    AllEmployees = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
                }
            }
        }

        public ICommand DeleteCommand
        {
            get { return new RelayCommand<int>(DeleteEmployee); }
        }

        public async void DeleteEmployee(int Id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.DeleteAsync("http://localhost:3655/api/employee/" + Id);
            }
            GetEmployees();
        }

        public ICommand EditCommand
        {
            get { return new RelayCommand<Employee>(EditEmployee); }
        }

        public void EditEmployee(Employee e)
        {
            if (e.Edit == Visibility.Visible)
            {
                e.Edit = Visibility.Collapsed;
            }
            else
            {
                e.Edit = Visibility.Visible;
            }
        }

        public ICommand UpdateCommand
        {
            get { return new RelayCommand<Employee>(UpdateEmployee); }
        }

        public async void UpdateEmployee(Employee e)
        {
            string input = JsonConvert.SerializeObject(e);

            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.PutAsync("http://localhost:3655/api/employee", new StringContent(input, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("error");
                }
                else
                {
                    EditEmployee(e);
                    GetEmployees();
                }
            }
        }

        public ICommand NewEmployeeCommand
        {
            get { return new RelayCommand(NewEmployee); }
        }

        public void NewEmployee()
        {
            Employee e = new Employee();
            e.EmployeeName = "naam";
            e.Email = "email";
            e.Address = "adres";
            e.Phone = "telefoonnummer";
            e.Edit = Visibility.Visible;
            AllEmployees.Add(e);
        }
    }
}
