using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PhoneBookWPF.Models
{
    public class User: IDataErrorInfo, INotifyPropertyChanged
    {
        private string firstName;
        
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

        public string LastName { get; set; } = string.Empty;
 
        public string Email { get; set; }

        public string Password{ get; set; } = string.Empty;

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        #endregion

        private UserValidator validator  = new UserValidator();

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
