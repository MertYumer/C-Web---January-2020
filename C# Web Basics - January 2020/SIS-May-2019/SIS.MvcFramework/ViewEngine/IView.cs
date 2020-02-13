namespace SIS.MvcFramework.ViewEngine
{
    using SIS.MvcFramework.Identity;
    using SIS.MvcFramework.Validation;

    public interface IView
    {
        string GetHtml(object model, ModelStateDictionary modelState, Principal user);
    }
}
