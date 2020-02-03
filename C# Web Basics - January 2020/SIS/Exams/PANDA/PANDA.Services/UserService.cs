namespace PANDA.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using PANDA.Data;
    using PANDA.Models;
    
    public class UserService : IUserService
    {
        private readonly PandaDbContext context;

        public UserService(PandaDbContext context)
        {
            this.context = context;
        }

        public User CreateUser(User user)
        {
            user = this.context.Users.Add(user).Entity;
            this.context.SaveChanges();

            return user;
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            var user = this.context
                .Users
                .SingleOrDefault(u => 
                (u.Username == username || u.Email == username)
                && u.Password == password);

            return user;
        }

        public IEnumerable<string> GetUsernames()
        {
            var usernames = this.context
                .Users
                .Select(x => x.Username)
                .ToList();

            return usernames;
        }
    }
}
