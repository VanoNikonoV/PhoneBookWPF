using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhoneBookWPF.Models
{
    public class User : IDataErrorInfo, INotifyPropertyChanged
    {
        private string firstName = string.Empty;
        public string FirstName 
        {   
            get => firstName;

            set
            {
                if (firstName == value) return;
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        private string lastName = string.Empty;
        public string LastName 
        {
            get => lastName;
            set
            { 
                if (lastName == value) return;
                lastName = value; 
                OnPropertyChanged(nameof(LastName));
            }
        } 

        private string email = string.Empty;
        public string Email 
        {
            get => email;

            set 
            { 
                if (email == value) return;
                email = value; 
                OnPropertyChanged(nameof(Email));
            }
        } 

        private string password = string.Empty;
        public string Password 
        {
            get => password;

            set 
            { 
                if (password == value) return;
                password = value;
                OnPropertyChanged(nameof(Password));
            } 
        } 

        private string confirmPassword = string.Empty;
        public string ConfirmPassword 
        {
            get => confirmPassword;
            
            set 
            { 
                if (confirmPassword == value) return;
                confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        } 

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion

        private UserValidator validator;

        [JsonIgnore]
        public string this[string columnName]
        {
            get
            {
                if (validator == null)
                {
                    validator = new UserValidator();
                }
                var firstOrDefault = validator.Validate(this)
                    .Errors.FirstOrDefault(lol => lol.PropertyName == columnName);
                return firstOrDefault?.ErrorMessage;
            }
        }
        [JsonIgnore]
        public string Error
        {
            get
            {
                var results = validator.Validate(this);

                if (results != null && results.Errors.Any())
                {
                    var errors = string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray());

                    return errors;
                }

                return string.Empty;
            }
        }
    }
}
