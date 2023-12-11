using PhoneBookWPF.Commands;
using PhoneBookWPF.Context;
using PhoneBookWPF.Models;
using PhoneBookWPF.View;
using PhoneBookWPF.View.Base;
using System.Collections.ObjectModel;
using System.Windows;

namespace PhoneBookWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private RequestLogin _requestlogin;

        public RequestLogin RequestLogin
        {  
            get => _requestlogin;

            set => Set(ref _requestlogin, value, "RequestLogin");
        }

        //public string StatusBarText { get; set; }

        public  IContactData Context { get; private set; }

        private ObservableCollection<IContact>? contactView;

        /// <summary>
        /// Коллекция контактов
        /// </summary>
        public ObservableCollection<IContact> ContactView
        {
            get
            {
                if (contactView == null)
                {                   
                    IEnumerable<IContact> temp = Contacts().Result;

                    contactView = new ObservableCollection<IContact>(temp);
                }
                return contactView;
            }
        }

        public MainWindowViewModel()
        {
            _requestlogin = new RequestLogin() { Email = "Не авторзованый пользователь"};

            Context = new ContactDataApi(_requestlogin);
        }

        #region Commands

        private RelayCommand loginCommand = null;

        public RelayCommand LoginCommand =>
            loginCommand ?? (loginCommand = new RelayCommand(Login, CanLogin));

        private RelayCommand exitCommand = null;
        public RelayCommand ExitCommand => exitCommand ?? (exitCommand = new RelayCommand(Exit, CanExit));


        private RelayCommand registerCommand = null;

        public RelayCommand RegisterCommand => registerCommand ?? (registerCommand = new RelayCommand(Register, CanLogin));

        #endregion

        private async void Register()
        {
            //this.enterBottun.Visibility = Visibility.Hidden;
            //string login = this.login.Text;
            //string password = this.password.Password;
            //string confirmPassword = this.confirmPassword.Password;

            //if (password.Equals(confirmPassword))
            //{

            //}
            //else { MessageBox.Show("Значения паролей не совпадаю!"); }
        }

        private bool CanExit()
        {
            bool flag = _requestlogin.IsToken ? true : false;

            return flag;
        }

        private bool CanLogin()
        {
            bool flag = _requestlogin.IsToken ? false : true;

            return flag;
        }

        private void Login()
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow(RequestLogin) { Owner = Application.Current.MainWindow};

            authorizationWindow.Show();
        }

        private void Exit()
        {
            this.RequestLogin.IsToken = false;

            this.RequestLogin.Email = "Не авторзованый пользователь";

            this.RequestLogin.Password = string.Empty;

            AccessForToken.Token = string.Empty;
        }

        private async Task<IEnumerable<IContact>> Contacts()
        {
            IEnumerable<IContact> temp = await Context.GetAllContact();

            return temp;
        }

        /// <summary>
        /// Метод удаляющий текст сообщения в StatusBar
        /// </summary>
        /// <param name="message">Текст информационного сообщения</param>
        //private void ShowStatusBarText(string message)
        //{
        //    StatusBarText = message;

        //    Window window = Application.Current.MainWindow;

        //    var timer = new System.Timers.Timer();

        //    timer.Interval = 2000;

        //    timer.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
        //    {
        //        timer.Stop();
        //        //удалите текст сообщения о состоянии с помощью диспетчера, поскольку таймер работает в другом потоке
        //        window.Dispatcher.BeginInvoke(new Action(() =>
        //        {
        //            StatusBarText = "";
        //        }));
        //    };
        //    timer.Start();
        //}
    }
}
