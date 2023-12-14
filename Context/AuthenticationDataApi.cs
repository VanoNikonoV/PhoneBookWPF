using Newtonsoft.Json;
using PhoneBookWPF.Models;
using System.Net;
using System.Net.Http;
using System.Text;

namespace PhoneBookWPF.Context
{

    public class AuthenticationDataApi :IAuthenticationData
    {
        public AuthenticationDataApi() { }

        private static readonly HttpClient httpClient = new()
        {
            BaseAddress = new Uri("https://a22611-1ae3.k.d-f.pw/") 
        };
        
        public async Task<(HttpStatusCode httpStatusCode, string responseText)> Login(RequestLogin request)
        {
            string serelizeContact = JsonConvert.SerializeObject(request);

            var response = await httpClient.PostAsync(
                requestUri: "authentication/login",
                content: new StringContent(serelizeContact, Encoding.UTF8,
                mediaType: "application/json"));

            if (response.IsSuccessStatusCode) 
            { 
                AccessForToken.Token = await response.Content.ReadAsStringAsync();

                return (response.StatusCode, "Вход выполнен успешно!");
            }
            else 
            {
                var error = await response.Content.ReadAsStringAsync(); // информация об ощибке

                string responseText = error;

                return (response.StatusCode, error);
            }
        }

        public async Task<(HttpStatusCode httpStatusCode, string responseText)> Register(User user)
        {
            UserDto userDto = new UserDto();

            userDto.FirstName = user.FirstName;
            userDto.LastName = user.LastName;

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            userDto.PasswordHash = passwordHash;

            userDto.Email = user.Email;

            string serelizeUser = JsonConvert.SerializeObject(userDto);

            var response = await httpClient.PostAsync(
                requestUri: "authentication/register",
                content: new StringContent(serelizeUser, Encoding.UTF8,
                mediaType: "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return (response.StatusCode, "Регистрация прошла успешено!");
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync(); // информация об ощибке

                string responseText = error;

                return (response.StatusCode, error);
            }

        }
    }
}
