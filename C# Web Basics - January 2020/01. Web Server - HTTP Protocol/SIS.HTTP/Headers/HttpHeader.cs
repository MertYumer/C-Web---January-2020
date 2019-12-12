namespace SIS.HTTP.Headers
{
    using Common;

    public class HttpHeader
    {
        public const string ContentType = "Content-Type";

        public const string Location = "Location";

        public HttpHeader(string key, string value)
        {
            key.ThrowIfNullOrEmpty(nameof(key));
            value.ThrowIfNullOrEmpty(nameof(value));

            this.Key = key;
            this.Value = value;
        }

        public string Key { get; set; }

        public string Value { get; set; }

        public override string ToString() => ($"{this.Key}: {this.Value}");
    }
}
