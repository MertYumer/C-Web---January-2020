namespace IRunes.App.Controllers
{
    using IRunes.App.ViewModels;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Http;
    using SIS.MvcFramework.Result;

    public class HomeController : Controller
    {
        [HttpGet(Url = "/")]
        public IActionResult IndexSlash()
        {
            return Index();
        }

        public IActionResult Index()
        {
            if (this.IsLoggedIn())
            {
                return this.View(new UserHomeViewModel { Username = this.User.Username }, "Home");
            }

            return this.View();
        }
    }
}
