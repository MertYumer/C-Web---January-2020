namespace SIS.HTTP.Requests.Contracts
{
    using System.Collections.Generic;

    using Enums;
    using Headers.Contracts;

    public interface IHttpRequest
    {
        string Path { get; }

        string Url { get; }

        Dictionary<string, ISet<string>> FormData { get; }

        Dictionary<string, ISet<string>> QueryData { get; }

        IHttpHeaderCollection Headers { get; }

        HttpRequestMethod RequestMethod { get; }
    }
}
