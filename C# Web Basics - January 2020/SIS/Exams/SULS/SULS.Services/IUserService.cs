namespace SULS.Services
{
    using SULS.Models;

    public interface IUserService
    {
        bool CreateUser(User user);

        User GetUserByUsernameAndPassword(string username, string password);
    }
}
