namespace SULS.Web.Controllers
{
    using SIS.HTTP;
    using SIS.MvcFramework;
    using SULS.Services;
    using SULS.Web.BindindModels.Users;

    public class UsersController : Controller
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public HttpResponse Register()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(UserRegisterBindingModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username) || 
                string.IsNullOrWhiteSpace(model.Password) ||
                string.IsNullOrWhiteSpace(model.ConfirmPassword) ||
                string.IsNullOrWhiteSpace(model.Email))
            {
                return this.Error("All fields are required.");
            }

            if (model.Username.Length < 4 || model.Username.Length > 10)
            {
                return this.Error("Username must be between 4 and 10 characters.");
            }

            if (model.Password.Length < 6 || model.Password.Length > 20)
            {
                return this.Error("Password must be between 6 and 20 characters.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.Error("Password should match.");
            }

            if (this.userService.UsernameExists(model.Username))
            {
                return this.Error("Username already in use.");
            }

            if (this.userService.EmailExists(model.Email))
            {
                return this.Error("Email already in use.");
            }

            this.userService.Register(model.Username, model.Email, model.Password);

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Login()
        {
            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(UserLoginBindingModel model)
        {
            var user = this.userService.GetUser(model.Username, model.Password);

            if (user == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SignIn(user.Id);

            return this.Redirect("/");
        }

        public HttpResponse Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }
    }
}
