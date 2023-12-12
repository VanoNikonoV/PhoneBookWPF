using FluentValidation;

namespace PhoneBookWPF.Models
{
    class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {

            RuleFor(user => user.FirstName)
                      .NotEmpty().WithMessage("Имя не заполнено")
                      .Must(user => user.All(Char.IsLetter)).WithMessage("Имя должно собержать только буквы")
                      .Must(StartsWithUpper).WithMessage("Имя должно начинаться с заглавной буквы");

            RuleFor(user => user.LastName)
                .NotEmpty().WithMessage("Фамилия не заполнена")
                .Must(user => user.All(Char.IsLetter)).WithMessage("Фамилия должно собержать только буквы")
                .Must(StartsWithUpper).WithMessage("Фамилия должно начинаться с заглавной буквы");

            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("Нужно указать значение")
                .EmailAddress().WithMessage("A valid email address is required."); ;

            RuleFor(user => user.Password).NotEmpty().WithMessage("Пароль не заполен");
                //.Equal(customer => customer.ConfirmPassword).WithMessage("Пароли не совпадают");

        }

        
        /// Определяет является ли первым символом 
        /// текущего экземпляра строки символ верхнего регистра
        /// </summary>
        /// <param name="suserr">Проеряемая строка</param>
        /// <reuserurns>true - если превая буква заглавная
        /// false - если превая буква незаглавная </returns>
        public bool StartsWithUpper(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;

            char ch = str[0];
            return char.IsUpper(ch);
        }

       
    }
}
