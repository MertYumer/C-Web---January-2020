namespace SIS.MvcFramework.ViewEngine
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class SisViewEngine : IViewEngine
    {
        public string GetHtml<T>(string viewContent, T model)
        {
            var csharpHtmlCode = this.GetCSharpCode(viewContent);

            var code = $@"
using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using SIS.MvcFramework.ViewEngine;
namespace AppViewCodeNamespace
{{
    public class AppViewCode : IView
    {{
        public string GetHtml(object model)
        {{
            var Model = model as {model.GetType().FullName};

            var html = new StringBuilder();

            {csharpHtmlCode}

            return html.ToString();
        }}
    }}
}}";

            var view = this.CompileAndInstance(code, model?.GetType().Assembly);
            var htmlResult = view?.GetHtml(model);
            return htmlResult;
        }

        private string GetCSharpCode(string viewContent)
        {
            var lines = viewContent.Split(new[] { "\r\n", "\n\r", "\n" }, StringSplitOptions.None);
            var cSharpCode = new StringBuilder();
            var supportedOperators = new[] { "for", "if", "else" };

            foreach (var line in lines)
            {
                if (line.TrimStart().StartsWith("{") || line.TrimStart().StartsWith("}"))
                {
                    //{ / }
                    cSharpCode.AppendLine(line);
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
                    if (!line.Contains("@"))
                    {
                        var csharpLine = $"html.AppendLine(@\"{line.Replace("\"", "\"\"")}\")";
                        cSharpCode.AppendLine(csharpLine);
                    }

                    else
                    {
                        var cSharpStringToAppend = "html.AppendLine(@\"";
                        var restOfLine = line;

                        while (restOfLine.Contains("@"))
                        {
                            var atSignLocation = restOfLine.IndexOf("@");
                            var plainText = restOfLine.Substring(0, atSignLocation);
                            var cSharpCodeRegex = new Regex(@"[^\s<""]+", RegexOptions.Compiled);
                            var cSharpExpression = cSharpCodeRegex.Match(restOfLine.Substring(atSignLocation + 1))?.Value;
                            cSharpStringToAppend += plainText + "\" + " + cSharpExpression + " + @\"";

                            if (restOfLine.Length <= atSignLocation + cSharpExpression.Length + 1)
                            {
                                restOfLine = string.Empty;
                            }

                            else
                            {
                                restOfLine = restOfLine.Substring(atSignLocation + cSharpExpression.Length + 1);
                            }
                        }

                        cSharpStringToAppend += $"{restOfLine}\");";
                        cSharpCode.AppendLine(cSharpStringToAppend);
                    }
                }
            }

            return cSharpCode.ToString();
        }

        private IView CompileAndInstance(string code, Assembly modelAssembly)
        {
            var compilation = CSharpCompilation.Create("AppViewAssembly")
                .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary))
                .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
                .AddReferences(MetadataReference.CreateFromFile(typeof(IView).Assembly.Location))
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

                    foreach (var error in errors)
                    {
                        Console.WriteLine(error.GetMessage());
                    }

                    return null;
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
