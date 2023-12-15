﻿using PhoneBookWPF.Commands;
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

        public RequestLogin RequestLogin
        {  
            get => _requestLogin;

            set => Set(ref _requestLogin, value, "RequestLogin");
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
        }

        private Contact currentContact;
        public Contact CurrentContac 
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

        private void Login()
        {
            AuthorizationWindow authorizationWindow = 
                new AuthorizationWindow(RequestLogin) { Owner = Application.Current.MainWindow };

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

        private async void DeleteContact()
        {
            int id = CurrentContac.Id;

            HttpStatusCode result = await Context.DeleteContact(id);

            if (result == HttpStatusCode.NotFound) { MessageBox.Show(result.ToString()); }

            if (result == HttpStatusCode.OK) { AccessForToken_onСhangedToken();}
        }

        private void AddContact()
        {
            //ValidationResult result = await _validator.ValidateAsync(contact);

            //if (!result.IsValid)
            //{
            //    result.AddToModelState(this.ModelState);

            //    return View(contact);
            //}
            //else
            //{
            //    HttpStatusCode statusCode = await _context.CreateContact(contact);

            //    if (statusCode == HttpStatusCode.Created) { return RedirectToAction(nameof(Index)); }
            //    if (contact == null || statusCode == HttpStatusCode.NotFound) { return NotFound(); }
            //    if (statusCode == HttpStatusCode.Unauthorized) { return RedirectToAction(nameof(NotAuthentication)); }

            //    return RedirectToAction(nameof(Index));
            //}
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
