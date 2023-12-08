using PhoneBookWPF.Context;
using PhoneBookWPF.Models;
using PhoneBookWPF.View.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookWPF.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
        private readonly IRequestLogin _login;

        public  IContactData Context { get; private set; }

        private ObservableCollection<IContact>? contactView;

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
            _login = new RequestLogin() { Email = "Не авторзован"};

            Context = new ContactDataApi(_login);
        }
        private async Task<IEnumerable<IContact>> Contacts()
        {

            IEnumerable<IContact> temp = await Context.GetAllContact();

            return temp.ToList();
        }

    }
}
