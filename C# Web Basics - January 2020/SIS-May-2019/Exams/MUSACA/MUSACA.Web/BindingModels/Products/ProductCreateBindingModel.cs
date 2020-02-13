namespace MUSACA.Web.BindingModels.Products
{
    using SIS.MvcFramework.Attributes.Validation;

    public class ProductCreateBindingModel
    {
        private const string NameErrorMessage = "Product Name must be between 5 and 20 symbols long.";

        private const string PriceErrorMessage = "Product Price must be greater than or equal to 0.01.";

        [RequiredSis]
        [StringLengthSis(3, 10, NameErrorMessage)]
        public string Name { get; set; }


        [RequiredSis]
        [RangeSis(typeof(decimal), "0,01", "79228162514264337593543950335", PriceErrorMessage)]
        public decimal Price { get; set; }
    }
}
