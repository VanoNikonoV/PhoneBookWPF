using PhoneBookWPF.Commands;
using PhoneBookWPF.Context;
using PhoneBookWPF.Models;
using PhoneBookWPF.View;
using PhoneBookWPF.View.Base;
using System.Collections.ObjectModel;
using System.Security.Principal;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace PhoneBookWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private RequestLogin _requestLogin = new RequestLogin();

        public RequestLogin RequestLogin
        {  
            get => _requestLogin;

            set
            {
                if (_requestLogin == value) return;
                _requestLogin = value;

                if (_requestLogin.IsToken) 
                {
                    IEnumerable<IContact> tempCollection = Contacts().Result;

                     this.ContactView = new ObservableCollection<IContact>(tempCollection);
                }
                if (!_requestLogin.IsToken)
                {
                    IEnumerable<IContact> tempCollection = Contacts().Result;

                    this.ContactView = HidingData(tempCollection);
                }
                base.OnPropertyChanged(nameof(RequestLogin));
            }
            //set => Set(ref _requestlogin, value, "RequestLogin");
        }

       

        //public string StatusBarText { get; set; }

        public  IContactData Context { get; private set; }

        private ObservableCollection<IContact>? contactView;

        /// <summary>
        /// Коллекция контактов
        /// </summary>
        public ObservableCollection<IContact> ContactView
        {
            get => contactView;

            set => Set(ref contactView, value, "ContactView");

            //get
            //{
            //    if (contactView == null)
            //    {                   
            //        IEnumerable<IContact> temp = Contacts().Result;

            //        contactView = new ObservableCollection<IContact>(temp);
            //    }
            //    return contactView;
            //}
        }

        public MainWindowViewModel()
        {
            //RequestLogin = new RequestLogin() { Email = "Не авторзованый пользователь"};

            //contactView = new ObservableCollection<IContact>();
            //this.RequestLogin = new RequestLogin() { Email = "Exit program" };

            Context = new ContactDataApi(this.RequestLogin);

            this.RequestLogin = new RequestLogin() { Email = "Проидите аутинтификацию"};

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

        private bool CanExit()
        {
            bool flag = RequestLogin.IsToken ? true : false;

            return flag;
        }

        private bool CanLogin()
        {
            bool flag = RequestLogin.IsToken ? false : true;

            return flag;
        }

        private void Login()
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow(RequestLogin) { Owner = Application.Current.MainWindow};

            authorizationWindow.Show();

            
        }

        private void Register()
        {
            RegisterWindow registerWindow = new RegisterWindow(){ Owner = Application.Current.MainWindow };

            registerWindow.Show();
        }

        private void Exit()
        {
            this.RequestLogin.IsToken = false;

            this.RequestLogin.Email = "Не авторзованый пользователь";

            this.RequestLogin.Password = string.Empty;

            AccessForToken.Token = string.Empty;
        }

        private ObservableCollection<IContact> HidingData(IEnumerable<IContact> tempCollection)
        {
            ObservableCollection<IContact> ContactsForAnonymous = new ObservableCollection<IContact>();

            foreach (IContact contact in tempCollection)
            {
                string outputTelefon = ConcealmentData(contact?.Telefon);
                string outputAddres = ConcealmentData(contact?.Address);
                string outputDisripion = ConcealmentData(contact?.Description);

                Contact outputContact = new Contact()
                {
                    Id = contact.Id,
                    FirstName = contact.FirstName,
                    MiddleName = contact.MiddleName,
                    LastName = contact.LastName,
                    Telefon = outputTelefon,
                    Address = outputAddres,
                    Description = outputDisripion
                };

                ContactsForAnonymous.Add(outputContact);
            }

            return ContactsForAnonymous;
        }

        /// <summary>
        /// Сокрыте данных контакта
        /// </summary>
        /// <param name="inputData">Входные данные</param>
        /// <returns>Скрытые данные либо "нет данных"</returns>
        private string ConcealmentData(string? inputData)
        {
            if (inputData == null) { return null; }

            if (inputData.Length > 0 && inputData != null && inputData != String.Empty)
            {
                string data = inputData;

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < inputData.Length; i++)
                {
                    if (data[i] != ' ')
                    {
                        sb.Append('*');
                    }
                    else sb.Append(data[i]);
                }
                return sb.ToString();
            }

            else return "нет данных";
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
