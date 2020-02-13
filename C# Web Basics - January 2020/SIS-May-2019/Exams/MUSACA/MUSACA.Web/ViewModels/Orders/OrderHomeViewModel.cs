namespace MUSACA.Web.ViewModels.Orders
{
    using System.Collections.Generic;
    using System.Linq;

    using MUSACA.Web.ViewModels.Products;

    public class OrderHomeViewModel
    {
        public OrderHomeViewModel()
        {
            this.Products = new List<ProductHomeViewModel>();
        }

        public List<ProductHomeViewModel> Products { get; set; }

        public decimal Price => this.Products.Sum(product => product.Price);
    }
}
