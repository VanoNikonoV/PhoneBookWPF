using Newtonsoft.Json;
using PhoneBook.Models;
using PhoneBookWPF.Context;
using System.ComponentModel;

namespace PhoneBookWPF.Models
{
    public class Contact : IContact, IDataErrorInfo
    {
        /// <summary>
        /// Идентификатор контакта
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary> 
        [JsonProperty("middleName")]
        public string MiddleName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [JsonProperty("lastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [JsonProperty("telefon")]
        public string Telefon { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// Описание 
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; set; }

        private ContactValidator validator;

        [JsonIgnore]
        public string this[string columnName]
        {
            get
            {
                if (validator == null)
                {
                    validator = new ContactValidator();
                }
                var firstOrDefault = validator.Validate(this)
                    .Errors.FirstOrDefault(lol => lol.PropertyName == columnName);
                return firstOrDefault?.ErrorMessage;
            }
        }

        [JsonIgnore]
        public string Error
        {
            get
            {
                var results = validator.Validate(this);

                if (results != null && results.Errors.Any())
                {
                    var errors = string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray());

                    return errors;
                }

                return string.Empty;
            }
        }
    }
}
