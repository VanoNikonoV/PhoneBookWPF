using PhoneBookWPF.Commands;
using PhoneBookWPF.Context;
using PhoneBookWPF.Models;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace PhoneBookWPF.ViewModels
{
    internal class AuthorizationWindowViewModel
    {
        private AuthenticationDataApi authentication;

        public AuthenticationDataApi Authentication
        {
            get { return authentication; }
        }

        private RequestLogin requestLogin;

        public RequestLogin RequestLogin
        {
            get => requestLogin;

            set
            {
                if (value == requestLogin) return;
                requestLogin = value;
            }

        }

        public string TextEmail { get; set; }

        public Window Owner { get; private set; }

        public AuthorizationWindowViewModel(RequestLogin requestLogin, Window owner)
        {
            authentication = new AuthenticationDataApi();

            this.requestLogin = requestLogin;

            this.Owner = owner;
        }

        private RelayCommandT<PasswordBox> _loginCommand = null;

        public RelayCommandT<PasswordBox> LoginCommand => _loginCommand ?? (_loginCommand  = new RelayCommandT<PasswordBox>(Login));


        private async void Login(PasswordBox passwordBox)
        {
            requestLogin.Email = TextEmail;
            requestLogin.Password = passwordBox.Password;

            
            var result = await authentication.Login(RequestLogin);
            HttpStatusCode httpStatusCode = result.httpStatusCode;
            string responseText = result.responseText;

            if (httpStatusCode == HttpStatusCode.OK)
            {
                requestLogin.IsToken = true;

                this.Owner.Close();
            }
            else
            {
                MessageBox.Show(responseText);
            }
        }
    }
}
