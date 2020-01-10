namespace IRunes.App.Controllers
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Runtime.CompilerServices;

    using IRunes.Models;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework.Result;

    public abstract class BaseController
    {
        public BaseController()
        {
            this.ViewData = new Dictionary<string, object>();
        }

        protected Dictionary<string, object> ViewData { get; set; }

        private string ParseTemplate(string viewContent)
        {
            foreach (var param in ViewData)
            {
                viewContent = viewContent.Replace($"@Model.{param.Key}", param.Value.ToString());
            }

            return viewContent;
        }

        protected bool IsLoggedIn(IHttpRequest httpRequest)
        {
            return httpRequest.Session.ContainsParameter("username");
        }

        protected void SignIn(IHttpRequest httpRequest, User user)
        {
            httpRequest.Session.AddParameter("username", user.Username);
            httpRequest.Session.AddParameter("email", user.Email);
            httpRequest.Session.AddParameter("id", user.Id);
        }

        protected void SignOut(IHttpRequest httpRequest)
        {
            httpRequest.Session.ClearParameters();
        }

        protected IHttpResponse View([CallerMemberName] string view = null)
        {
            var controllerName = this.GetType().Name.Replace("Controller", "");
            var viewName = view;
            var content = File.ReadAllText("Views/" + controllerName + "/" + viewName + ".html");

            content = ParseTemplate(content);

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }

        protected IHttpResponse Redirect(string location)
        {
            return new RedirectResult(location);
        }

        protected bool IsValid(object obj)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(obj);

            return Validator.TryValidateObject(obj, validationContext, validationResults, true);
        }
    }
}
