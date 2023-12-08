using PhoneBookWPF.Context;
using PhoneBookWPF.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PhoneBookWPF.View
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window, INotifyPropertyChanged
    {
        private AuthenticationDataApi authentication;

        public AuthenticationDataApi Authentication 
        { 
            get { return authentication; } 
        }
        private RequestLogin requestLogin;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public RequestLogin RequestLogin 
        {   
            get => requestLogin;
            
            set 
            { 
                if(value == requestLogin) return; 
                requestLogin = value;
                OnPropertyChanged(nameof(RequestLogin));
            }
        
        }


        public AuthorizationWindow(RequestLogin requestLogin)
        {
            authentication = new AuthenticationDataApi();

            this.requestLogin = requestLogin;

            InitializeComponent();

        }
        private async void Button_Click_Enter(object sender, RoutedEventArgs e)
        {
            requestLogin.Email = this.login.Text;
            requestLogin.Password = this.password.Password;

            HttpStatusCode httpStatusCode = await authentication.Login(RequestLogin);

            if (httpStatusCode == HttpStatusCode.OK) 
            { 
                requestLogin.IsToken = true;

                this.Close(); 
            }
        }

        private void Button_Click_Reg(object sender, RoutedEventArgs e)
        {
            this.enterBottun.Visibility = Visibility.Hidden;
            string login = this.login.Text;
            string password = this.password.Password;
            string confirmPassword = this.confirmPassword.Password;

            if (password.Equals(confirmPassword))
            {
               
            }
            else { MessageBox.Show("Значения паролей не совпадаю!"); }
        }
    }
}
