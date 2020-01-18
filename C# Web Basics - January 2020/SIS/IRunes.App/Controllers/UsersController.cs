namespace IRunes.App.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    using IRunes.Data;
    using IRunes.Models;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Action;
    using SIS.MvcFramework.Attributes.Http;
    using SIS.MvcFramework.Result;

    public class UsersController : Controller
    {
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost(ActionName = "Register")]
        public IActionResult RegisterConfirm()
        {
            using (var context = new RunesDbContext())
            {
                var username = ((ISet<string>)this.Request.FormData["username"]).FirstOrDefault();
                var password = ((ISet<string>)this.Request.FormData["password"]).FirstOrDefault();
                var confirmPassword = ((ISet<string>)this.Request.FormData["confirmPassword"]).FirstOrDefault();
                var email = ((ISet<string>)this.Request.FormData["email"]).FirstOrDefault();

                if (password != confirmPassword)
                {
                    return this.Redirect("/Users/Register");
                }

                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = username,
                    Password = this.HashPassword(password),
                    Email = email
                };

                context.Users.Add(user);
                context.SaveChanges();
            }

            return this.Redirect("/Users/Login");
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost(ActionName = "Login")]
        public IActionResult LoginConfirm()
        {
            using (var context = new RunesDbContext())
            {
                var username = ((ISet<string>)this.Request.FormData["username"]).FirstOrDefault();
                var password = ((ISet<string>)this.Request.FormData["password"]).FirstOrDefault();
                var hashedPassword = this.HashPassword(password);

                var userFromDb = context.Users
                    .FirstOrDefault(u => (u.Username == username || u.Email == username) && u.Password == hashedPassword);

                if (userFromDb == null)
                {
                    return this.Redirect("/Users/Login");
                }

                this.SignIn(userFromDb.Id, userFromDb.Username, userFromDb.Email);
            }

            return this.Redirect("/");
        }

        public IActionResult Logout()
        {
            this.SignOut();
            return this.Redirect("/");
        }

        [NonAction]
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }
    }
}
