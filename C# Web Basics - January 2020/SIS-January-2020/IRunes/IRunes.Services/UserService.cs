namespace IRunes.Services
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    using IRunes.Data;
    using IRunes.Models;
    using SIS.MvcFramework;

    public class UserService : IUserService
    {
        private readonly RunesDbContext context;

        public UserService(RunesDbContext context)
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

        public string GetUsername(string id)
        {
            var user = this.context.Users.SingleOrDefault(u => u.Id == id);

            if (user == null)
            {
                return null;
            }

            return user.Username;
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
