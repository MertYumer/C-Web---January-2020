namespace SIS.HTTP.Headers
{
    using Common;

    public class HttpHeader
    {
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
