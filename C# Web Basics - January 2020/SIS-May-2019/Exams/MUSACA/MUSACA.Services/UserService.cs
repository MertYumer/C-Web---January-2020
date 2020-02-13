namespace MUSACA.Services
{
    using System.Linq;

    using MUSACA.Data;
    using MUSACA.Models;

    public class UserService : IUserService
    {
        private readonly MusacaDbContext context;

        public UserService(MusacaDbContext context)
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
    }
}
