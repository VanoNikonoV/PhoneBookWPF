using PhoneBookWPF.Models;
using System.Net;

namespace PhoneBookWPF.Context
{
    public interface IAuthenticationData
    {
        Task<(HttpStatusCode httpStatusCode, string responseText)> Login(RequestLogin request);

        Task<HttpStatusCode> Register(User user);
    }
}
