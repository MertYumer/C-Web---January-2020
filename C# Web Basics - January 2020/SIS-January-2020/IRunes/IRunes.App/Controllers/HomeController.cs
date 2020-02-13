namespace IRunes.App.Controllers
{
    using IRunes.App.ViewModels.Home;
    using IRunes.Services;
    using SIS.HTTP;
    using SIS.MvcFramework;

    public class HomeController : Controller
    {
        private readonly IUserService userService;

        public HomeController(IUserService usersService)
        {
            this.userService = usersService;
        }

        [HttpGet("/")]
        public HttpResponse IndexSlash()
        {
            return Index();
        }

        public HttpResponse Index()
        {
            var viewModel = new IndexViewModel();
            viewModel.Username = "";

            if (this.IsUserLoggedIn())
            {
                viewModel.Username = this.userService.GetUsername(this.User);
            }

            return this.View(viewModel);
        }
    }
}
