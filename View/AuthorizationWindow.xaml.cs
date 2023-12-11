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
        private AuthorizationWindowViewModel authorizationWindowViewModel;

        public AuthorizationWindow(RequestLogin requestLogin)
        {
            authorizationWindowViewModel = new(requestLogin, this);

            InitializeComponent();

            this.DataContext = authorizationWindowViewModel;
        }
    }
}
