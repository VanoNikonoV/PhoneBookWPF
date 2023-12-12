using Newtonsoft.Json;
using PhoneBookWPF.Context;
using System.ComponentModel.DataAnnotations;

namespace PhoneBookWPF.Models
{
    public class Contact : IContact
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
  
    }
}
