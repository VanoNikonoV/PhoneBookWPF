using PhoneBookWPF.ViewModels;
using System.Windows;

namespace PhoneBookWPF.View
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterViewModel RegisterViewModel { get; private set; }

        public RegisterWindow()
        {
            RegisterViewModel = new RegisterViewModel();

            InitializeComponent();

            this.DataContext = RegisterViewModel;
            
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.password.Password == this.confirmPassword.Password)
            {
                RegisterViewModel.PasswordBox = this.password.Password;
                this.alert.Text = string.Empty;
            }
            else this.alert.Text = "Пароли не совпадают";
        }
    }
}
