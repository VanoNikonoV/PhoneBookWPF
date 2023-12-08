using PhoneBookWPF.Models;
using System.Net;

namespace PhoneBookWPF.Context
{
    public interface IAuthenticationData
    {
        Task<HttpStatusCode> Login(RequestLogin request);

        Task<HttpStatusCode> Register(User user);
    }
}
