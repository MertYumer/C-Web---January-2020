﻿namespace SIS.HTTP.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Common;
    using Contracts;
    using Enums;
    using Headers;
    using Headers.Contracts;
    using SIS.HTTP.Exceptions;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            requestString.ThrowIfNullOrEmpty(nameof(requestString));

            this.FormData = new Dictionary<string, ISet<string>>();
            this.QueryData = new Dictionary<string, ISet<string>>();
            this.Headers = new HttpHeaderCollection();

            this.ParseRequest(requestString);
        }

        public string Path { get; private set; }

        public string Url { get; private set; }

        public Dictionary<string, ISet<string>> FormData { get; }

        public Dictionary<string, ISet<string>> QueryData { get; }

        public IHttpHeaderCollection Headers { get; }

        public HttpRequestMethod RequestMethod { get; private set; }

        private bool IsValidRequestLine(string[] requestLineParams)
            => requestLineParams.Length == 3 && requestLineParams[2] == GlobalConstants.HttpOneProtocolFragment;

        private bool IsValidRequestQueryString(string queryString, string[] queryParameters)
            => !string.IsNullOrEmpty(queryString) && queryParameters.Length >= 1;

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
            => this.Url = requestLineParams[1];

        private void ParseRequestPath()
            => this.Path = this.Url.Split('?')[0];

        private void ParseRequestHeaders(string[] plainHeaders)
        {
            plainHeaders
                .Select(header => header.Split(new[] { ": " }, StringSplitOptions.RemoveEmptyEntries))
                .ToList()
                .ForEach(headerKeyValuePair
                => this.Headers.AddHeader(new HttpHeader(headerKeyValuePair[0], headerKeyValuePair[1])));
        }

        private void ParseCookies()
        {
            
        }

        private void ParseQueryParameters()
        {
            var parameters = this.Url
                .Split('?', '#')[1]
                .Split('&')
                .Select(plainQueryParameter => plainQueryParameter.Split('='))
                .ToList();

            foreach (var parameter in parameters)
            {
                if (this.Url.Split('?').Length > 1)
                {
                    if (!this.QueryData.ContainsKey(parameter[0]))
                    {
                        this.QueryData.Add(parameter[0], new HashSet<string>());
                    }

                    this.QueryData[parameter[0]].Add(WebUtility.UrlDecode(parameter[1]));
                }
            }
        }

        private void ParseFormDataParameters(string requestBody)
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

        private void ParseRequestParameters(string requestBody)
        {
            this.ParseQueryParameters();
            this.ParseFormDataParameters(requestBody);
        }

        private void ParseRequest(string requestString)
        {
            var splitRequestContent = requestString
                .Split(new[] { GlobalConstants.HttpNewLine }, StringSplitOptions.None);

            var requestLineParams = splitRequestContent[0]
                .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            if (!this.IsValidRequestLine(requestLineParams))
            {
                throw new BadRequestException();
            }

            this.ParseRequestMethod(requestLineParams);
            this.ParseRequestUrl(requestLineParams);
            this.ParseRequestPath();

            this.ParseRequestHeaders(splitRequestContent.Skip(1).ToArray());
            this.ParseCookies();

            this.ParseRequestParameters(splitRequestContent[^1]);
        }
    }
}
