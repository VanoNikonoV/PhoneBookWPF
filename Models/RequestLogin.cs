using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PhoneBookWPF.Models
{
    public class RequestLogin: IRequestLogin, INotifyPropertyChanged
    {
        private string email = string.Empty;
        public string Email 
        {  
            get => email;

            set
            {
                if (value == email) return;
                email = value;
                OnPropertyChanged(nameof(Email));
            }
            
        } 

        public string Password { get; set; } = string.Empty;

        private bool isToken = false;
        public bool IsToken 
        {
            get => isToken;

            set
            {
                if (value == isToken) return;
                isToken = value;
                OnPropertyChanged(nameof(IsToken));
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
