using FluentValidation;
using FluentValidation.Results;
using PhoneBookWPF.Commands;
using PhoneBookWPF.Context;
using PhoneBookWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PhoneBookWPF.ViewModels
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string PasswordBox { get; set; } = string.Empty;

        private UserValidator _validator;

        public AuthenticationDataApi Authentication { get; private set; }

        private User user;

        public RegisterViewModel()
        {
            Authentication = new();

            user = new();

            _validator = new();
        }

        private RelayCommand registrCommand = null;

        public RelayCommand RegisterCommand => registrCommand ?? (registrCommand = new RelayCommand(Register, CanRegister));

        private bool CanRegister()
        {
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Email = Email;
            user.Password = PasswordBox;

            var result = _validator.Validate(user);

          

            if (result.IsValid) return true;
            else return false;


        }

        private async void Register()
        {
           HttpStatusCode httpStatusCode = await this.Authentication.Register(user);
        }
    }
}
