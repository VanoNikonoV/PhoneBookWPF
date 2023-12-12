using PhoneBookWPF.Models;
using PhoneBookWPF.ViewModels;
using System.Windows;

namespace PhoneBookWPF.View
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        private AuthorizationViewModel authorizationViewModel;

        public AuthorizationWindow(RequestLogin requestLogin)
        {
            authorizationViewModel = new(requestLogin, this);

            InitializeComponent();

            this.DataContext = authorizationViewModel;
        }
    }
}
