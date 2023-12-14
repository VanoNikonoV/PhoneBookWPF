using System.Security;
using System.Windows;
using System.Windows.Controls;

namespace PhoneBookWPF.View.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        public static readonly DependencyProperty PasswordProperty = 
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox));
        
        public string Password
        {
            get {  return (string)GetValue(PasswordProperty); } //SecureString 
            set { SetValue(PasswordProperty, value); }
        }
        public BindablePasswordBox()
        {
            InitializeComponent();
            textPassword.PasswordChanged += OnPasswordChanged;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
           Password = textPassword.Password;
        }
    }
}
