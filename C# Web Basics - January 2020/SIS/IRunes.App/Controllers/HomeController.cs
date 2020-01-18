namespace IRunes.App.Controllers
{
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
                this.ViewData.Add("Username", this.User.Username);
                return this.View("/Index-Logged");
            }

            return this.View();
        }
    }
}
