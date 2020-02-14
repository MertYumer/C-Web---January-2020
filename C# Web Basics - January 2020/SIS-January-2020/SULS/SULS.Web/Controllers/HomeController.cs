namespace SULS.Web.Controllers
{
    using SIS.HTTP;
    using SIS.MvcFramework;
    using SULS.Web.ViewModels.Problems;

    public class HomeController : Controller
    {
        [HttpGet("/")]
        public HttpResponse IndexSlash()
        {
            return Index();
        }

        public HttpResponse Index()
        {
            var viewModel = new ProblemAllViewModel();

            if (this.IsUserLoggedIn())
            {

            }

            return this.View(viewModel);
        }
    }
}
