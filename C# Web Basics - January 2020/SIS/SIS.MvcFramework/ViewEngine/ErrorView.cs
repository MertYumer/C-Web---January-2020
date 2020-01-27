
namespace SIS.MvcFramework.ViewEngine
{
    using SIS.MvcFramework.Identity;

    public class ErrorView : IView
    {
        private readonly string errors;

        public ErrorView(string errors)
        {
            this.errors = errors;
        }

        public string GetHtml(object model, Principal user)
        {
            return this.errors;
        }
    }
}
