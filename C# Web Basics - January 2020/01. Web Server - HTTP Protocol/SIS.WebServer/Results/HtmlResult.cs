﻿namespace SIS.WebServer.Results
{
    using System.Text;

    using HTTP.Enums;
    using HTTP.Headers;
    using HTTP.Responses;

    public class HtmlResult : HttpResponse
    {
        public HtmlResult(string content, HttpResponseStatusCode responseStatusCode)
            : base(responseStatusCode)
        {
            this.Content = Encoding.UTF8.GetBytes(content);
            this.Headers.AddHeader(new HttpHeader(HttpHeader.ContentType, "text/html; charset=utf-8"));
        }
    }
}
