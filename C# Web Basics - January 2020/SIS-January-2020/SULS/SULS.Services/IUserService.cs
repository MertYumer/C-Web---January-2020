namespace SULS.Services
{
    using SULS.Models;

    public interface IUserService
    {
        void Register(string username, string email, string password);

        User GetUser(string username, string password);

        string GetUsername(string id);

        bool UsernameExists(string username);

        bool EmailExists(string email);
    }
}
