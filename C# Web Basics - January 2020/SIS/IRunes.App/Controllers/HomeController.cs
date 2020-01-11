namespace IRunes.App.Controllers
{
    using SIS.HTTP.Requests;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;

    public class HomeController : Controller
    {
        public IHttpResponse Index(IHttpRequest httpRequest)
        {
            if (this.IsLoggedIn(httpRequest))
            {
                this.ViewData.Add("Username", httpRequest.Session.GetParameter("username").ToString());
                return this.View("/Index-Logged");
            }

            return this.View();
        }
    }
}
