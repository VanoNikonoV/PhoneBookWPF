using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace PhoneBookWPF.Context
{
    public interface IContact
    {
        /// <summary>
        /// Идентификатор контакта
        /// </summary>
        [JsonProperty("id")]
        int Id { get; set; }
        /// <summary>
        /// Имя
        /// </summary>

        [JsonProperty("firstName")]
        string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        [JsonProperty("middleName")]
        string MiddleName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        [JsonProperty("lastName")]
        string LastName { get; set; }

        /// <summary>
        /// Номер телефона
        /// </summary>
        [JsonProperty("telefon")]
        string Telefon { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }
        /// <summary>
        /// Описание 
        /// </summary>
        ///  /// </summary>
        [JsonProperty("description")]
        string? Description { get; set; }
    }
}
