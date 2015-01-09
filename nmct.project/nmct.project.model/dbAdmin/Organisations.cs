using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.model
{
    public class Organisations
    {
        #region "properties"

        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _login;
        [Required(ErrorMessage="Een loginnaam is verplicht")]
        [DisplayName("Login")]
        [MinLength(2, ErrorMessage="De naam moet minimum 2 letters bevatten")]
        public string Login
        {
            get { return _login; }
            set { _login = value; }
        }
        
        private string _password;
        [Required(ErrorMessage = "Een wachtwoord is verplicht")]
        [DisplayName("Wachtwoord")]
        [MinLength(5, ErrorMessage = "Het wachtwoord moet minimum 5 karakters bevatten")]
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        private string _dbName;
        [Required(ErrorMessage = "Een databasenaam is verplicht")]
        [DisplayName("Naam van de database")]
        [MinLength(2, ErrorMessage = "De naam moet minimum 2 letters bevatten")]
        public string DbName
        {
            get { return _dbName; }
            set { _dbName = value; }
        }
        
        private string _dbPassword;
        [Required(ErrorMessage = "Een wachtwoord is verplicht")]
        [DisplayName("Database wachtwoord")]
        [MinLength(2, ErrorMessage = "Het wachtwoord moet minimum 5 karakters bevatten")]
        public string DbPassword
        {
            get { return _dbPassword; }
            set { _dbPassword = value; }
        }

        private string _dbLogin;
        [Required(ErrorMessage = "Een loginnaam is verplicht")]
        [DisplayName("Database login")]
        [MinLength(2, ErrorMessage = "De naam moet minimum 2 letters bevatten")]
        public string DbLogin
        {
            get { return _dbLogin; }
            set { _dbLogin = value; }
        }
        
        private string _organisationName;
        [Required(ErrorMessage = "Een organisatienaam is verplicht")]
        [DisplayName("Organisatie")]
        [MinLength(2, ErrorMessage = "De naam moet minimum 2 letters bevatten")]
        public string OrganisationName
        {
            get { return _organisationName; }
            set { _organisationName = value; }
        }
        
        private string _address;
        [Required(ErrorMessage = "Een adres is verplicht")]
        [DisplayName("Adres")]
        public string Address
        {
            get { return _address; }
            set { _address = value; }
        }
        
        private string _email;
        [Required(ErrorMessage = "Een emailadres is verplicht")]
        [DisplayName("Emailadres")]
        [DataType(DataType.EmailAddress)]
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        
        private string _phone;
        [Required(ErrorMessage = "Een telefoonnummer is verplicht")]
        [DisplayName("Telefoonnummer")]
        public string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }

        #endregion
    }
}
