namespace PANDA.Web.ViewModels.Packages
{
    using SIS.MvcFramework.Attributes.Validation;

    public class PackageCreateInputModel
    {
        private const string DescriptionErrorMessage = "Description must be between 5 and 20 symbols!";

        [RequiredSis]
        [StringLengthSis(5, 20, DescriptionErrorMessage)]
        public string Description { get; set; }

        public decimal Weight { get; set; }

        public string ShippingAddress { get; set; }

        [RequiredSis]
        public string RecipientName { get; set; }
    }
}
