using System.Net;

namespace PhoneBookWPF.Context
{
    public interface IContactData
    {
        Task<IEnumerable<IContact>> GetAllContact();
        Task<(IContact, HttpStatusCode)> GetContact(int? id);
        Task<HttpStatusCode> CreateContact(IContact newContact);
        Task<HttpStatusCode> UpdateContact(int id, IContact contact);
        Task<HttpStatusCode> DeleteContact(int id);
    }
}
