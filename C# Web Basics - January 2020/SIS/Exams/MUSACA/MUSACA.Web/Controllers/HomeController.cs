namespace MUSACA.Web.Controllers
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
            return this.View();
        }
    }
}
