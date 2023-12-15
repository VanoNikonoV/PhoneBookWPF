using PhoneBookWPF.Commands;
using PhoneBookWPF.Context;
using PhoneBookWPF.Models;
using System.Net;
using System.Windows;


namespace PhoneBookWPF.ViewModels
{
    public class RegisterViewModel
    {
         public AuthenticationDataApi Authentication { get; private set; }

        public User User { get; set; }

        public Window Owner { get; private set; }

        public RegisterViewModel(Window owner)
        {
            Authentication = new();

            User = new();

            this.Owner = owner;
        }

        private RelayCommand registrCommand = null;

        public RelayCommand RegisterCommand => registrCommand ?? (registrCommand = new RelayCommand(Register, CanRegister));

        private bool CanRegister()
        {
            if (User.Error == string.Empty)
            {
                return true;
            }
            else { return false; }
        }

        private async void Register()
        {
            var result = await  this.Authentication.Register(User);
            HttpStatusCode httpStatusCode = result.httpStatusCode;
            string responseText = result.responseText;

            if (httpStatusCode == HttpStatusCode.OK)
            {
                MessageBox.Show(responseText);

                this.Owner.Close();
            }
            else
            {
                MessageBox.Show(responseText);
            }
        }
    }
}
