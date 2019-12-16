namespace SIS.WebServer.Results
{
    using System.Text;

    using HTTP.Enums;
    using HTTP.Headers;
    using HTTP.Responses;

    public class TextResult : HttpResponse
    {
        public TextResult(string content, HttpResponseStatusCode responseStatusCode,
            string contentType = "text/plain; charset=utf-8") 
            : base(responseStatusCode)
        {
            this.Content = Encoding.UTF8.GetBytes(content);
            this.Headers.AddHeader(new HttpHeader(HttpHeader.ContentType, contentType));
        }

        public TextResult(byte[] content, HttpResponseStatusCode responseStatusCode,
            string contentType = "text/plain; charset=utf-8")
        {
            this.Content = content;
            this.Headers.AddHeader(new HttpHeader(HttpHeader.ContentType, contentType));
        }
    }
}
