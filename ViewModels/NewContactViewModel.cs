using PhoneBookWPF.Commands;
using PhoneBookWPF.Models;
using PhoneBookWPF.View.Base;
using System.Windows;

namespace PhoneBookWPF.ViewModels
{
    internal class NewContactViewModel: ViewModel
    {
        public NewContactViewModel(Window ownerWindow)
        {
            NewContact = new()
            { 
                FirstName = "Имя",
                LastName = "Фамилия",
                MiddleName = "Отчество",
                Telefon = "+79876543210",
                Address = "г.Екатеринбург",
                Description = "Описание"
            };
      
            this.ownerWindow = ownerWindow;
        }

        public Contact NewContact { get; }

        private Window ownerWindow;

        private RelayCommand saveContactCommand = null;
        public RelayCommand SaveContactCommand =>
            saveContactCommand ?? (saveContactCommand = new(SaveContact, CanSaveContact));

        private bool CanSaveContact()
        {
            if (NewContact.Error == string.Empty)
            {
                return true;
            }
            else { return false; }
        }

        private void SaveContact() { ownerWindow.DialogResult = true; }
    }
}
