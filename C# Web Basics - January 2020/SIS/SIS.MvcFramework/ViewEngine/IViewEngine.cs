namespace SIS.MvcFramework.ViewEngine
{
    using SIS.MvcFramework.Identity;
    using SIS.MvcFramework.Validation;

    public interface IViewEngine
    {
        string GetHtml<T>(string viewContent, T model, ModelStateDictionary modelState, Principal user);
    }
}

