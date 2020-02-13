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

        public bool CreateUser(User user)
        {
            this.context.Users.Add(user);
            this.context.SaveChanges();

            return true;
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
