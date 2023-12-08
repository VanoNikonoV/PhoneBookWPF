namespace PhoneBookWPF.Models
{
    public interface IRequestLogin
    {
        string Email { get; set; }

        string Password { get; set; }

        bool IsToken { get; set; }
    }
}
