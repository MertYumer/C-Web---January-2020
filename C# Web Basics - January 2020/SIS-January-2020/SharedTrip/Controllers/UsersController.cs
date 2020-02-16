namespace SharedTrip.Controllers
{
    using SharedTrip.BindindModels;
    using SharedTrip.Services;
    using SIS.HTTP;
    using SIS.MvcFramework;

    public class UsersController : Controller
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public HttpResponse Register()
        {
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/Trips/All");
            }

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
                return this.View();
            }

            if (model.Username.Length < 5 || model.Username.Length > 20)
            {
                return this.View();
            }

            if (model.Password.Length < 6 || model.Password.Length > 20)
            {
                return this.View();
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.View();
            }

            if (this.userService.UsernameExists(model.Username))
            {
                return this.View();
            }

            if (this.userService.EmailExists(model.Email))
            {
                return this.View();
            }

            this.userService.Register(model);

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Login()
        {
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/Trips/All");
            }

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
