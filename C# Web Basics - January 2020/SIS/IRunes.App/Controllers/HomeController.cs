namespace IRunes.App.Controllers
{
    using SIS.HTTP.Requests;
    using SIS.HTTP.Responses;

    public class HomeController : BaseController
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
