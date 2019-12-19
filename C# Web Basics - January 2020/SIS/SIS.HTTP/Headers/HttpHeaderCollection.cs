namespace SIS.HTTP.Headers
{
    using System.Collections.Generic;
    using System.Linq;

    using Contracts;
    using Common;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly Dictionary<string, HttpHeader> httpHeaders;

        public HttpHeaderCollection()
        {
            this.httpHeaders = new Dictionary<string, HttpHeader>();
        }

        public void AddHeader(HttpHeader header)
        {
            header.ThrowIfNull(nameof(header));
            this.httpHeaders.Add(header.Key, header);
        }

        public bool ContainsHeader(string key)
        {
            key.ThrowIfNullOrEmpty(key);
            return this.httpHeaders.ContainsKey(nameof(key));
        }

        public HttpHeader GetHeader(string key)
        {
            key.ThrowIfNullOrEmpty(nameof(key));
            return this.httpHeaders[key];
        }

        public override string ToString()
            => string.Join(GlobalConstants.HttpNewLine, httpHeaders.Values.Select(header => header.ToString()));
    }
}