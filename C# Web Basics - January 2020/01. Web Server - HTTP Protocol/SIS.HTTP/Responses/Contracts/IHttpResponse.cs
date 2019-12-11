namespace SIS.HTTP.Responses.Contracts
{
    using Enums;
    using Headers;

    public interface IHttpResponse
    {
        HttpResponseStatusCode StatusCode { get; set; }

        HttpHeaderCollection Headers { get; }

        byte[] Content { get; set; }

        void AddHeader(HttpHeader header);

        byte[] GetBytes();
    }
}
