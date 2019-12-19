namespace SIS.HTTP.Requests.Contracts
{
    using System.Collections.Generic;

    //using SIS.HTTP.Cookies.Contracts;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers.Contracts;
    //using SIS.HTTP.Sessions.Contracts;

    public interface IHttpRequest
    {
        string Url { get; }

        string Path { get; }

        Dictionary<string, ISet<string>> FormData { get; }

        Dictionary<string, ISet<string>> QueryData { get; }

        IHttpHeaderCollection Headers { get; }

        HttpRequestMethod RequestMethod { get; }

        //IHttpCookieCollection Cookies { get; }

        //IHttpSession Session { get; set; }
    }
}