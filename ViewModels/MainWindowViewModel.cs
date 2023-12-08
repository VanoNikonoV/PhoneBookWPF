using PhoneBookWPF.Commands;
using PhoneBookWPF.Context;
using PhoneBookWPF.Models;
using PhoneBookWPF.View;
using PhoneBookWPF.View.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _requestlogin = new RequestLogin() { Email = "Не авторзован"};

            Context = new ContactDataApi(_requestlogin);
        }

        #region Commands

        private RelayCommand loginCommand = null;

        public RelayCommand LoginCommand =>
            loginCommand ?? (loginCommand = new RelayCommand(Login, CanLogin));

        private bool CanLogin()
        {
            return true;
        }

        private void Login()
        {
            AuthorizationWindow authorizationWindow = new AuthorizationWindow(RequestLogin) { Owner = Application.Current.MainWindow};

            authorizationWindow.Show();
        }

        #endregion

        private async Task<IEnumerable<IContact>> Contacts()
        {
            IEnumerable<IContact> temp = await Context.GetAllContact();

            return temp;
        }

    }
}
