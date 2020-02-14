namespace SULS.Services
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    using SIS.MvcFramework;
    using SULS.Data;
    using SULS.Models;

    public class UserService : IUserService
    {
        private readonly SulsDbContext context;

        public UserService(SulsDbContext context)
        {
            this.context = context;
        }

        public void Register(string username, string email, string password)
        {
            var user = new User
            {
                Role = IdentityRole.User,
                Username = username,
                Email = email,
                Password = this.Hash(password),
            };

            this.context.Users.Add(user);
            this.context.SaveChanges();
        }

        public User GetUser(string username, string password)
        {
            var user = this.context
                .Users
                .SingleOrDefault(u =>
                (u.Username == username || u.Email == username)
                && u.Password == Hash(password));

            return user;
        }

        public User GetUserById(string id)
        {
            var user = this.context.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public bool UsernameExists(string username)
        {
            return this.context.Users.Any(x => x.Username == username);
        }

        public bool EmailExists(string email)
        {
            return this.context.Users.Any(x => x.Email == email);
        }

        private string Hash(string input)
        {
            if (input == null)
            {
                return null;
            }

            var crypt = new SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(input));

            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}
