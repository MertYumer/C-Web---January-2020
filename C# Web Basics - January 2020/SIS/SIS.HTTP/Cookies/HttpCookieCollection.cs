namespace SIS.HTTP.Cookies
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Text;
	
	using SIS.HTTP.Common;
	using SIS.HTTP.Cookies.Contracts;
	
    public class HttpCookieCollection : IHttpCookieCollection
    {
        private readonly Dictionary<string, HttpCookie> httpCookies;

        public HttpCookieCollection()
        {
            this.httpCookies = new Dictionary<string, HttpCookie>();
        }

        public void AddCookie(HttpCookie cookie)
        {
			CoreValidator.ThrowIfNull(cookie, nameof(cookie));
			this.httpCookies.Add(cookie.Key, cookie);
		}

        public bool ContainsCookie(string key)
        {
			CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
			return this.httpCookies.ContainsKey(key);
		}

        public HttpCookie GetCookie(string key)
        {
			if (!this.ContainsCookie(key))
            {
                throw new ArgumentException("Invalid key!");
            }
			
			return this.httpCookies[key];
		}

        public bool HasCookies()
        {
			return this.httpCookies.Count > 0;
		}

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var cookie in this.httpCookies.Values)
            {                
                sb.Append($"Set-Cookie: {cookie}").Append(GlobalConstants.HttpNewLine);
            }

            return sb.ToString();
        }

        public IEnumerator<HttpCookie> GetEnumerator()
        {
            return httpCookies.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}