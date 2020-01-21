using System;

namespace SIS.MvcFramework.ViewEngine
{
    public class SisViewEngine : IViewEngine
    {
        public string GetHtml<T>(string viewContent, T model)
        {
            var csharpHtmlCode = this.GetCSharpCode();

            var code = $@"
namespace AppViewCodeNamespace
{{
    using System;
    using System.Net;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    
    using SIS.MvcFramework.ViewEngine;
    using SIS.MvcFramework.Identity;
    using SIS.MvcFramework.Validation;

    public class AppViewCode : IView
    {{
        public string GetHtml(object model, ModelStateDictionary modelState, Principal user)
        {{
            var html = new StringBuilder();

            {csharpHtmlCode}

            return html.ToString();
        }}
    }}
}}";

            var view = this.CompileAndInstance(code);
            //var htmlResult = view.GetHtml();
            return code;
        }

        private object CompileAndInstance(string code)
        {
            return string.Empty;
        }

        private string GetCSharpCode()
        {
            return string.Empty;
        }
    }
}
