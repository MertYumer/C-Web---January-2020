namespace SIS.HTTP.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    using SIS.HTTP.Common;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Cookies.Contracts;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Exceptions;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Headers.Contracts;
    using SIS.HTTP.Requests.Contracts;
    //using SIS.HTTP.Sessions;
    //using SIS.HTTP.Sessions.Contracts;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            requestString.ThrowIfNullOrEmpty(nameof(requestString));

            this.FormData = new Dictionary<string, ISet<string>>();
            this.QueryData = new Dictionary<string, ISet<string>>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, ISet<string>> FormData { get; }

        public Dictionary<string, ISet<string>> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public IHttpCookieCollection Cookies { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        private bool IsValidRequestLine(string[] requestLineParams)
            => requestLineParams.Length == 3 && requestLineParams[2] == GlobalConstants.HttpOneProtocolFragment;

        private void ParseRequestQueryParameters()
        {
            if (this.Url.Split('?').Length > 1)
            {
                var parameters = this.Url
                    .Split('?')[1]
                    .Split('&')
                    .Select(plainQueryParameter => plainQueryParameter.Split('='))
                    .ToList();

                foreach (var parameter in parameters)
                {
                    if (!this.QueryData.ContainsKey(parameter[0]))
                    {
                        this.QueryData.Add(parameter[0], new HashSet<string>());
                    }

                    this.QueryData[parameter[0]].Add(WebUtility.UrlDecode(parameter[1]));
                }
            }
        }

        private void ParseRequestFormDataParameters(string requestBody)
        {
            if (!string.IsNullOrEmpty(requestBody))
            {
                var parameters = requestBody
                    .Split('&')
                    .Select(plainQueryParameter => plainQueryParameter.Split('='))
                    .ToList();

                foreach (var parameter in parameters)
                {
                    if (!this.FormData.ContainsKey(parameter[0]))
                    {
                        this.FormData.Add(parameter[0], new HashSet<string>());
                    }

                    this.FormData[parameter[0]].Add(WebUtility.UrlDecode(parameter[1]));
                }
            }
        }

        private void ParseRequestMethod(string[] requestLineParams)
        {
            bool parseResult = Enum.TryParse(requestLineParams[0], true, out HttpRequestMethod method);

            if (!parseResult)
            {
                throw new BadRequestException(string.Format(
                    GlobalConstants.UnsupportedHttpMethodExceptionMessage,
                    requestLineParams[0]));
            }

            this.RequestMethod = method;
        }

        private void ParseRequestUrl(string[] requestLineParams)
            => this.Url = requestLineParams[1].Split('#')[0];

        private void ParseRequestPath()
            => this.Path = this.Url.Split('?')[0];

        private void ParseRequestHeaders(string[] requestLineParams)
        {
            foreach (var line in requestLineParams)
            {
                if (string.IsNullOrEmpty(line))
                {
                    break;
                }

                var headerKvp = line.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries);
                var header = new HttpHeader(headerKvp[0], headerKvp[1]);
                Headers.AddHeader(header);
            }
        }

        private void ParseRequestCookies()
        {
            if (!this.Headers.ContainsHeader("Cookie"))
            {
                return;
            }

            var cookies = Headers
            .GetHeader("Cookie")
            .Value
            .Split(new[] { "; " }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var cookie in cookies)
            {
                var cookieKvp = cookie.Split('=');
                var key = cookieKvp[0];
                var value = cookieKvp[1];

                this.Cookies.AddCookie(new HttpCookie(key, value, false));
            }
        }

        private void ParseRequestParameters(string requestBody)
        {
            this.ParseRequestQueryParameters();
            this.ParseRequestFormDataParameters(requestBody);
        }

        private void ParseRequest(string requestString)
        {
            var splitRequestContent = requestString
                .Split(new[] { GlobalConstants.HttpNewLine }, StringSplitOptions.None);

            var requestLineParams = splitRequestContent[0]
                .Trim()
                .Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLineParams))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLineParams);
            this.ParseRequestUrl(requestLineParams);
            this.ParseRequestPath();

            this.ParseRequestHeaders(splitRequestContent.Skip(1).ToArray());
            this.ParseRequestCookies();

            this.ParseRequestParameters(splitRequestContent[splitRequestContent.Length - 1]);
        }
    }
}