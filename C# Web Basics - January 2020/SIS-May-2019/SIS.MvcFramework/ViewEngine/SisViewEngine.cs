namespace SIS.MvcFramework.ViewEngine
{
    using System;
    using System.Collections;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;

    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using SIS.MvcFramework.Identity;
    using SIS.MvcFramework.Validation;

    public class SisViewEngine : IViewEngine
    {
        public string GetHtml<T>(string viewContent, T model, ModelStateDictionary modelState, Principal user = null)
        {
            string csharpHtmlCode = string.Empty;
            csharpHtmlCode = this.CheckForWidgets(viewContent);
            csharpHtmlCode = this.GetCSharpCode(csharpHtmlCode);

            var code = $@"
using System;
using System.Net;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SIS.MvcFramework.ViewEngine;
using SIS.MvcFramework.Identity;
using SIS.MvcFramework.Validation;
namespace AppViewCodeNamespace
{{
    public class AppViewCode : IView
    {{
        public string GetHtml(object model, ModelStateDictionary modelState, Principal user)
        {{
            var Model = {(model == null ? "new {}" : "model as " + GetModelType(model))};
            var User = user;  
            var ModelState= modelState;

            var html = new StringBuilder();

            {csharpHtmlCode}

            return html.ToString();
        }}
    }}
}}";

            var view = this.CompileAndInstance(code, model?.GetType().Assembly);
            var htmlResult = view?.GetHtml(model, modelState, user);
            return htmlResult;
        }

        private string CheckForWidgets(string viewContent)
        {
            var widgets = Assembly
                .GetEntryAssembly()?
                .GetTypes()
                .Where(type => typeof(IViewWidget).IsAssignableFrom(type))
                .Select(x => (IViewWidget)Activator.CreateInstance(x))
                .ToList();

            if (widgets == null || widgets.Count == 0)
            {
                return viewContent;
            }

            string widgetPrefix = "@Widgets.";

            foreach (var viewWidget in widgets)
            {
                viewContent = viewContent.Replace($"{widgetPrefix}{viewWidget.GetType().Name}", viewWidget.Render());
            }

            return viewContent;
        }

        private string GetModelType<T>(T model)
        {
            if (model is IEnumerable)
            {
                return $"IEnumerable<{model.GetType().GetGenericArguments()[0].FullName}>";
            }

            return model.GetType().FullName;
        }

        private string GetCSharpCode(string viewContent)
        {
            var lines = viewContent.Split(new[] { "\r\n", "\n\r", "\n" }, StringSplitOptions.None);
            var cSharpCode = new StringBuilder();
            var supportedOperators = new[] { "for", "if", "else" };
            var cSharpCodeRegex = new Regex(@"[^\s<""\&]+", RegexOptions.Compiled);
            var cSharpCodeDepth = 0;

            foreach (var line in lines)
            {
                if (line.TrimStart().StartsWith("@{"))
                {
                    cSharpCodeDepth++;
                }

                else if (line.TrimStart().StartsWith("{") || line.TrimStart().StartsWith("}"))
                {
                    //{ / }
                    if (cSharpCodeDepth > 0)
                    {
                        if (line.TrimStart().StartsWith("{"))
                        {
                            cSharpCodeDepth++;
                        }

                        else if (line.TrimStart().StartsWith("}"))
                        {
                            if ((--cSharpCodeDepth) == 0)
                            {
                                continue;
                            }
                        }
                    }

                    cSharpCode.AppendLine(line);
                }

                else if (cSharpCodeDepth > 0)
                {
                    cSharpCode.AppendLine(line);
                    continue;
                }

                else if (supportedOperators.Any(x => line.TrimStart().StartsWith("@" + x)))
                {
                    //@C#
                    var atSignLocation = line.IndexOf("@");
                    var csharpLine = line.Remove(atSignLocation, 1);
                    cSharpCode.AppendLine(csharpLine);
                }

                else
                {
                    //HTML
                    if (line.Contains("@RenderBody()"))
                    {
                        var csharpLine = $"html.AppendLine(@\"{line}\");";
                        cSharpCode.AppendLine(csharpLine);
                    }

                    else
                    {
                        var cSharpStringToAppend = "html.AppendLine(@\"";
                        var restOfLine = line;

                        while (restOfLine.Contains("@"))
                        {
                            var atSignLocation = restOfLine.IndexOf("@");
                            var plainText = restOfLine.Substring(0, atSignLocation).Replace("\"", "\"\""); ;
                            var cSharpExpression = cSharpCodeRegex.Match(restOfLine.Substring(atSignLocation + 1))?.Value;

                            if (cSharpExpression.Contains("{") && cSharpExpression.Contains("}"))
                            {
                                var csharpInlineExpression =
                                    cSharpExpression.Substring(1, cSharpExpression.IndexOf("}") - 1);

                                cSharpStringToAppend += plainText + "\" + " + csharpInlineExpression + " + @\"";

                                cSharpExpression = cSharpExpression.Substring(0, cSharpExpression.IndexOf("}") + 1);
                            }

                            else
                            {
                                cSharpStringToAppend += plainText + "\" + " + cSharpExpression + " + @\"";
                            }


                            if (restOfLine.Length <= atSignLocation + cSharpExpression.Length + 1)
                            {
                                restOfLine = string.Empty;
                            }

                            else
                            {
                                restOfLine = restOfLine.Substring(atSignLocation + cSharpExpression.Length + 1);
                            }
                        }

                        cSharpStringToAppend += $"{restOfLine.Replace("\"", "\"\"")}\");";
                        cSharpCode.AppendLine(cSharpStringToAppend);
                    }
                }
            }

            return cSharpCode.ToString();
        }

        private IView CompileAndInstance(string code, Assembly modelAssembly)
        {
            modelAssembly = modelAssembly ?? Assembly.GetEntryAssembly();

            var compilation = CSharpCompilation.Create("AppViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(Object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(Assembly.GetEntryAssembly().Location))
                .AddReferences(MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName("netstandard")).Location))
                .AddReferences(MetadataReference.CreateFromFile(modelAssembly.Location));

            var netStandardAssembly = Assembly.Load(new AssemblyName("netstandard")).GetReferencedAssemblies();

            foreach (var assembly in netStandardAssembly)
            {
                compilation = compilation.AddReferences(
                    MetadataReference.CreateFromFile(Assembly.Load(assembly).Location));
            }

            compilation = compilation.AddSyntaxTrees(SyntaxFactory.ParseSyntaxTree(code));

            using (var memoryStream = new MemoryStream())
            {
                var compilationResult = compilation.Emit(memoryStream);

                if (!compilationResult.Success)
                {
                    var errors = compilationResult.Diagnostics.Where(x => x.Severity == DiagnosticSeverity.Error);
                    var errorsHtml = new StringBuilder();
                    errorsHtml.AppendLine($"<h1>{errors.Count()} errors:</h1>");

                    foreach (var error in errors)
                    {
                        errorsHtml.AppendLine($"<div>{error.Location} => {error.GetMessage()}</div>");
                    }

                    errorsHtml.AppendLine($"<pre>{WebUtility.HtmlEncode(code)}</pre>");

                    return new ErrorView(errorsHtml.ToString());
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                var assemblyBytes = memoryStream.ToArray();
                var assembly = Assembly.Load(assemblyBytes);

                var type = assembly.GetType("AppViewCodeNamespace.AppViewCode");

                if (type == null)
                {
                    Console.WriteLine("AppViewCode not found.");
                    return null;
                }

                var instance = Activator.CreateInstance(type);

                return instance as IView;
            }
        }
    }
}
