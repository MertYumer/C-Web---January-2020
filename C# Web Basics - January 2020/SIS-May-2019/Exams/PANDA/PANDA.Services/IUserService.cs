namespace PANDA.Services
{
    using System.Collections.Generic;

    using PANDA.Models;

    public interface IUserService
    {
        User CreateUser(User user);

        User GetUserByUsernameAndPassword(string username, string password);

        IEnumerable<string> GetUsernames();
    }
}
