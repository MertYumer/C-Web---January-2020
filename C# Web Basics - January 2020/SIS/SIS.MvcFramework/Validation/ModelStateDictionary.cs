namespace SIS.MvcFramework.Validation
{
    using System.Collections.Generic;
    using System.Collections.Immutable;

    public class ModelStateDictionary
    {
        private readonly IDictionary<string, List<string>> errorMessages;

        public ModelStateDictionary()
        {
            this.errorMessages = new Dictionary<string, List<string>>();
            this.IsValid = true;
        }

        public IReadOnlyDictionary<string, List<string>> ErrorMessages
            => this.errorMessages.ToImmutableDictionary();

        public bool IsValid { get; set; }

        public void Add(string propertyName, string errorMessage)
        {
            if (!this.errorMessages.ContainsKey(propertyName))
            {
                this.errorMessages.Add(propertyName, new List<string>());
            }

            this.errorMessages[propertyName].Add(errorMessage);
        }
    }
}
