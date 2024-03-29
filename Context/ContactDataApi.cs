﻿using Newtonsoft.Json;
using System.Text;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using PhoneBookWPF.Models;

namespace PhoneBookWPF.Context
{
    /// <summary>
    /// Класс посредник с web-Api. Отвечающий за основные операция с данными: чтение, запись ....
    /// </summary>
    public class ContactDataApi : IContactData
    {
        private readonly IRequestLogin _login;

        private readonly Uri baseAddress = new Uri("https://a22611-1ae3.k.d-f.pw/");

        public ContactDataApi(IRequestLogin login) 
        {
            _login = login;
        }

        /// <summary>
        /// Производит удаление контакта из базы данных
        /// </summary>
        /// <param name="id">индентификатор контакта</param>
        /// <returns>HttpStatusCode</returns>
        public async Task<HttpStatusCode> DeleteContact(int id)
        {
            if (!_login.IsToken) return HttpStatusCode.Unauthorized;
            try
            {
                using (var client = new HttpClient() { BaseAddress = baseAddress })
                {
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + AccessForToken.Token);

                    string url = $"values/id?id={id}";

                    HttpResponseMessage response = await client.DeleteAsync(url);

                    return response.StatusCode;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return  HttpStatusCode.NotFound;
            }

        }

        /// <summary>
        /// Получает все контаты из базы даных, использую web API
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<IContact>> GetAllContact()
        {
            using (var client = new HttpClient() { BaseAddress = baseAddress })
            {
                string url = $"values";

                try
                {
                    var httpResponseMessage = client.GetAsync(url);

                    var respons = httpResponseMessage.Result.EnsureSuccessStatusCode();

                    if(respons.IsSuccessStatusCode)
                    {
                        string json = await respons.Content.ReadAsStringAsync();

                        IEnumerable<IContact> contacts = JsonConvert.DeserializeObject<IEnumerable<Contact>>(json);

                        return contacts;
                    }

                    else return null; //contacts;
                }
                catch (Exception exception) 
                { 
                    Debug.WriteLine(exception.Message);
                    return Enumerable.Empty<IContact>();
                }
            }   
        }

        /// <summary>
        /// Возвращает данные о контакте по его индентификатору, использую web API
        /// </summary>
        /// <param name="id">индентификатор контакта</param>
        /// <returns>Десерилозованный контакт</returns>
        public async Task<(IContact, HttpStatusCode)> GetContact(int? id) 
        {
            if (!_login.IsToken) return (NullContact.Create(), HttpStatusCode.Unauthorized);
            
            try
            {
                using (var client = new HttpClient() { BaseAddress = baseAddress }) 
                {
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + AccessForToken.Token);

                    string url = $"values/id?id=" + $"{id}";

                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonContact = await response.Content.ReadAsStringAsync();

                        IContact contact = JsonConvert.DeserializeObject<Contact>(jsonContact);

                        return (contact, response.StatusCode);
                    }
                    else { return (NullContact.Create(), response.StatusCode); }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                return (NullContact.Create(), HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return (NullContact.Create(), HttpStatusCode.NotFound);
            }
        }

        /// <summary>
        /// Производит обновление данных контакта
        /// </summary>
        /// <param name="id">индентификатор контакта</param>
        /// <param name="contact"></param>
        /// <returns>IContact - измененный контакт в случае успешного соединения
        /// NullContact - в случае не поладок в соединении</returns>
        public async Task<HttpStatusCode> UpdateContact(int id, IContact contact)
        {
            if (!_login.IsToken) return HttpStatusCode.Unauthorized;

            try
            {
                using (var client = new HttpClient() { BaseAddress = baseAddress })
                {
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Add("Authorization","Bearer " + AccessForToken.Token);

                    string url = $"values/id?id=" + $"{id}";

                    string serelizeContact = JsonConvert.SerializeObject(contact);

                    var response = await client.PutAsync(
                    requestUri: url,
                    content: new StringContent(serelizeContact, Encoding.UTF8,
                    mediaType: "application/json"));

                    if (response.IsSuccessStatusCode) { return  response.StatusCode; }

                    else { return response.StatusCode; }
                }
            }

            catch (HttpRequestException http) 
            {
                Debug.WriteLine(http.Message);
                return HttpStatusCode.NotFound;
            }

            catch (Exception ex) 
            {
                Debug.WriteLine(ex.Message);
                return  HttpStatusCode.NotFound;
            }
           
        }

        /// <summary>
        /// Добавляет контакт в базу данных, используя web API
        /// </summary>
        /// <param name="newContact"></param>
        public async Task<HttpStatusCode> CreateContact(IContact newContact)
        {
            if(!_login.IsToken) return HttpStatusCode.Unauthorized;

            try
            {
                using (var client = new HttpClient() { BaseAddress = baseAddress })
                {
                    client.DefaultRequestHeaders.Accept.Clear();

                    client.DefaultRequestHeaders.Add("Authorization", "bearer " + AccessForToken.Token);

                    string url = $"values";

                    string serelizeContact = JsonConvert.SerializeObject(newContact);

                    var response = await client.PostAsync(
                    requestUri: url,
                    content: new StringContent(serelizeContact, Encoding.UTF8,
                    mediaType: "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        return response.StatusCode;
                    }
                    else { return response.StatusCode; }
                }
            }

            catch (HttpRequestException http)
            {
                Debug.WriteLine(http.Message);
                return  HttpStatusCode.NotFound;
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return HttpStatusCode.NotFound;
            }

        }
    }
}
