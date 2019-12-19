namespace SIS.HTTP.Sessions
{
    using System;
    using System.Collections.Generic;

    using SIS.HTTP.Common;
    using SIS.HTTP.Sessions.Contracts;

    public class HttpSession : IHttpSession
    {
        private readonly Dictionary<string, object> sessionParameters;

        public HttpSession(string id)
        {
            this.Id = id;
            this.sessionParameters = new Dictionary<string, object>();
        }

        public string Id { get; }

        public void AddParameter(string name, object parameter)
        {
            CoreValidator.ThrowIfNullOrEmpty(name, nameof(name));
            CoreValidator.ThrowIfNull(parameter, nameof(parameter));

            this.sessionParameters[name] = parameter;
        }

        public void ClearParameters()
        {
            this.sessionParameters.Clear();
        }

        public bool ContainsParameter(string name)
        {
            CoreValidator.ThrowIfNullOrEmpty(name, nameof(name));
            return sessionParameters.ContainsKey(name);
        }

        public object GetParameter(string name)
        {
            if (!this.ContainsParameter(name))
            {
                throw new ArgumentException("Invalid parameter!");
            }

            return this.sessionParameters[name];
        }
    }
}
