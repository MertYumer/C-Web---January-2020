namespace MUSACA.Web.Controllers
{
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    using MUSACA.Models;
    using MUSACA.Services;
    using MUSACA.Web.BindingModels.Users;
    using MUSACA.Web.ViewModels.Orders;
    using MUSACA.Web.ViewModels.Users;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Action;
    using SIS.MvcFramework.Attributes.Http;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.Mapping;
    using SIS.MvcFramework.Result;

    public class UsersController : Controller
    {
        private readonly IUserService userService;
        private readonly IOrderService orderService;

        public UsersController(IUserService userService, IOrderService orderService)
        {
            this.userService = userService;
            this.orderService = orderService;
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Users/Register");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.Redirect("/Users/Register");
            }

            var user = ModelMapper.ProjectTo<User>(model);
            user.Password = this.HashPassword(model.Password);

            this.userService.CreateUser(user);
            this.orderService.CreateOrder(new Order { CashierId = user.Id });
            this.SignIn(user.Id, user.Username, user.Password);

            return this.Redirect("/");
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Users/Login");
            }

            var hashedPassword = this.HashPassword(model.Password);

            var userFromDb = this.userService.GetUserByUsernameAndPassword(model.Username, hashedPassword);

            if (userFromDb == null)
            {
                return this.Redirect("/Users/Login");
            }

            this.SignIn(userFromDb.Id, userFromDb.Username, userFromDb.Email);

            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.SignOut();
            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Profile()
        {
            var orders = this.orderService.GetAllCompletedOrdersByCashierId(this.User.Id);

            var userProfileViewModel = new UserProfileViewModel();
            userProfileViewModel.Orders = orders
                .Select(ModelMapper.ProjectTo<OrderProfileViewModel>)
                .ToList();

            return this.View(userProfileViewModel);
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
