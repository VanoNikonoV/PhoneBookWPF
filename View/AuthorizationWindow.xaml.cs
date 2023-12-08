using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
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
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
        }
        private void Button_Click_Enter(object sender, RoutedEventArgs e)
        {
            string login = this.login.Text;
            string password = this.password.Password;

            string querystring = $"select id_user, login_user, password_user from register where login_user = '{login}' and password_user = '{password}'";

           
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
