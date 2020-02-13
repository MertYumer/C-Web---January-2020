namespace MUSACA.Services
{
    using MUSACA.Models;

    public interface IUserService
    {
        User CreateUser(User user);

        User GetUserByUsernameAndPassword(string username, string password);
    }
}
