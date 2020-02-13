namespace MUSACA.Web.BindingModels.Products
{
    using SIS.MvcFramework.Attributes.Validation;

    public class ProductOrderBindingModel
    {
        [RequiredSis]
        public string Product { get; set; }
    }
}
