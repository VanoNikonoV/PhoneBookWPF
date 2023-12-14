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
            RegisterViewModel = new RegisterViewModel(this);

            InitializeComponent();

            this.DataContext = RegisterViewModel;
            
        }
    }
}
