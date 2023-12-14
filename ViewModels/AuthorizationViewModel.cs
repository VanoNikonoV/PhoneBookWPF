using PhoneBookWPF.Commands;
using PhoneBookWPF.Context;
using PhoneBookWPF.Models;
using PhoneBookWPF.View.Base;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace PhoneBookWPF.ViewModels
{
    internal class AuthorizationViewModel: ViewModel
    {
        public AuthenticationDataApi Authentication { get; private set; }

        private RequestLogin requestLogin;

        public RequestLogin RequestLogin
        {
            get => requestLogin;

            set => Set(ref requestLogin, value, "RequestLogin");
        }

        public string TextEmail { get; set; }

        public Window Owner { get; private set; }

        public AuthorizationViewModel(RequestLogin requestLogin, Window owner)
        {
            Authentication = new AuthenticationDataApi();

            this.requestLogin = requestLogin;

            this.Owner = owner;
        }

        private RelayCommandT<PasswordBox> _loginCommand = null;
        public RelayCommandT<PasswordBox> LoginCommand => 
            _loginCommand ?? (_loginCommand  = new RelayCommandT<PasswordBox>(Login));

        private async void Login(PasswordBox passwordBox)
        {
            RequestLogin.Email = TextEmail;
            RequestLogin.Password = passwordBox.Password;
            //this.RequestLogin = new RequestLogin() { Email = TextEmail, Password = passwordBox.Password };

            var result = await Authentication.Login(RequestLogin);
            HttpStatusCode httpStatusCode = result.httpStatusCode;
            string responseText = result.responseText;

            if (httpStatusCode == HttpStatusCode.OK)
            {
                RequestLogin.IsToken = true;

                //MessageBox.Show(responseText);

                this.Owner.Close();
            }
            else
            {
                MessageBox.Show(responseText);
            }
        }
    }
}
