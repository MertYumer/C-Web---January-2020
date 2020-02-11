namespace SULS.Services
{
    using System.Linq;

    using SULS.Data;
    using SULS.Models;

    public class UserService : IUserService
    {
        private readonly SulsDbContext context;

        public UserService(SulsDbContext context)
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
