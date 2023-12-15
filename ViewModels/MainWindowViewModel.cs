using PhoneBookWPF.Commands;
using PhoneBookWPF.Context;
using PhoneBookWPF.Models;
using PhoneBookWPF.View;
using PhoneBookWPF.View.Base;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using System.Windows;

namespace PhoneBookWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        public MainWindowViewModel()
        {
            Context = new ContactDataApi(this.RequestLogin);

            this.RequestLogin = new RequestLogin() { Email = "Проидите аутинтификацию" };

            AccessForToken.onСhangedToken += AccessForToken_onСhangedToken;

            IEnumerable<IContact> tempCollection = Context.GetAllContact().Result;

            this.ContactView = HidingData(tempCollection);
        }

        private RequestLogin _requestLogin = new RequestLogin();

        /// <summary>
        /// Пользователь приложения, связан с логикой работы контекста (получение данных)
        /// </summary>
        public RequestLogin RequestLogin
        {  
            get => _requestLogin;

            set => Set(ref _requestLogin, value, "RequestLogin");
        }

        private string statusBarText = "Здесь будет подсказка";
        public string StatusBarText 
        {
            get => statusBarText; 
            set => Set(ref statusBarText, value, "StatusBarText");
        } 

        /// <summary>
        /// Источник данных для приложения
        /// </summary>
        public  IContactData Context { get; private set; }
        

        private ObservableCollection<IContact>? contactView;

        /// <summary>
        /// Коллекция контактов для отображения во View
        /// </summary>
        public ObservableCollection<IContact> ContactView
        {
            get => contactView;

            set => Set(ref contactView, value, "ContactView");
        }

        private Contact currentContact;
        /// <summary>
        /// Выбранный клинет - DataGrid
        /// </summary>
        public Contact CurrentContact 
        {   
            get => currentContact;
            set => Set(ref currentContact, value, "CurrentContac");
        }

        /// <summary>
        /// Обновление списка после смены пользователя, изменения данных в коллекции
        /// </summary>
        private async void AccessForToken_onСhangedToken()
        {
            if (!(AccessForToken.Token == string.Empty)) 
            {
                Context = new ContactDataApi(this.RequestLogin);

                IEnumerable<IContact> tempCollection = await Context.GetAllContact();

                this.ContactView = new ObservableCollection<IContact>(tempCollection);
            }
            else
            {
                IEnumerable<IContact> tempCollection = await Context.GetAllContact();

                this.ContactView = HidingData(tempCollection);
            }
        }

        #region Commands

        private RelayCommand loginCommand = null;
        public RelayCommand LoginCommand =>
            loginCommand ?? (loginCommand = new RelayCommand(Login, CanLogin));


        private RelayCommand exitCommand = null;
        public RelayCommand ExitCommand =>
            exitCommand ?? (exitCommand = new RelayCommand(Exit, CanExit));


        private RelayCommand registerCommand = null;
        public RelayCommand RegisterCommand => 
            registerCommand ?? (registerCommand = new RelayCommand(Register, CanLogin));

        private RelayCommand deleteContactCommand = null;
        public RelayCommand DeleteContactCommand => 
            deleteContactCommand ?? (deleteContactCommand = new RelayCommand(DeleteContact, CanExit));

        private RelayCommand addContactCommand = null;
        public RelayCommand AddContactCommand => 
            addContactCommand ?? (addContactCommand = new RelayCommand(AddContact, CanExit));

        private RelayCommand editContactCommand = null;
        public RelayCommand EditContactCommand => 
            editContactCommand ?? (editContactCommand = new RelayCommand(EditContact, CanExit));

        #endregion

        #region Методы для команд
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

        /// <summary>
        /// Логика входа в программу
        /// </summary>
        private void Login()
        {
            AuthorizationWindow authorizationWindow = 
                new AuthorizationWindow(RequestLogin) { Owner = Application.Current.MainWindow };

            authorizationWindow.Show(); 
        }

        /// <summary>
        /// Логика регистрации нового пользователя
        /// </summary>
        private void Register()
        {
            RegisterWindow registerWindow = new RegisterWindow(){ Owner = Application.Current.MainWindow };

            registerWindow.Show();
        }

        /// <summary>
        /// Выход из аккаунта
        /// </summary>
        private void Exit()
        {
            this.RequestLogin.IsToken = false;

            this.RequestLogin.Email = "Не авторзованый пользователь";

            this.RequestLogin.Password = string.Empty;

            AccessForToken.Token = string.Empty;
        }
        /// <summary>
        /// Удаление выбранной записи
        /// </summary>
        private async void DeleteContact()
        {
            int id = CurrentContact.Id;

            HttpStatusCode result = await Context.DeleteContact(id);

            if (result == HttpStatusCode.OK) { AccessForToken_onСhangedToken(); }

            if (result == HttpStatusCode.Forbidden) { ShowStatusBarText("Отказано в доступе!"); }

            if (result == HttpStatusCode.NotFound) { ShowStatusBarText(result.ToString()); }
        }

        /// <summary>
        /// Добавление выбранной записи
        /// </summary>
        private async void AddContact()
        {
            NewContactWindow newContactWindow = new NewContactWindow() { Owner = Application.Current.MainWindow};

            NewContactViewModel newContactViewModel = new(newContactWindow);

            newContactWindow.DataContext = newContactViewModel;

            newContactWindow.ShowDialog();

            if (newContactWindow.DialogResult == true)
            {
               HttpStatusCode httpStatusCode =  await Context.CreateContact(newContactViewModel.NewContact);

                if (httpStatusCode == HttpStatusCode.OK) { AccessForToken_onСhangedToken(); }

                else 
                {
                    ShowStatusBarText(httpStatusCode.ToString());
                    AccessForToken_onСhangedToken();
                }
            }
        }

        /// <summary>
        /// Редактирование выбранной записи
        /// </summary>
        private async void EditContact()
        {
            Contact currentContact = CurrentContact;
            int id = CurrentContact.Id;

            HttpStatusCode httpStatusCode = await Context.UpdateContact(id, currentContact);     

            if (httpStatusCode == HttpStatusCode.OK) { AccessForToken_onСhangedToken(); }

            if (httpStatusCode == HttpStatusCode.Forbidden) { ShowStatusBarText("Отказано в доступе!"); }

            else { ShowStatusBarText(httpStatusCode.ToString()); }
        }

        #endregion

        /// <summary>
        /// Сокрытие некоторых данных о контакте
        /// </summary>
        /// <param name="tempCollection">Коллекция контактов</param>
        /// <returns>Коллекция контактов со скрытими данными</returns>
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
        /// Сокрыте данных (одного поля) контакта
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

        /// <summary>
        /// Метод удаляющий текст сообщения в StatusBar
        /// </summary>
        /// <param name="message">Текст информационного сообщения</param>
        private void ShowStatusBarText(string message)
        {
            StatusBarText = message;

            Window window = Application.Current.MainWindow;

            var timer = new System.Timers.Timer();

            timer.Interval = 2500;

            timer.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
            {
                timer.Stop();
                
                StatusBarText = "Здесь будет подсказка";
            };
            timer.Start();
        }
    }
}


