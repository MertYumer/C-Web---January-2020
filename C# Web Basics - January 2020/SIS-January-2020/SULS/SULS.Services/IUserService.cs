namespace SULS.Services
{
    using SULS.Models;

    public interface IUserService
    {
        void Register(string username, string email, string password);

        User GetUser(string username, string password);

        User GetUserById(string userId);

        bool UsernameExists(string username);

        bool EmailExists(string email);
    }
}
