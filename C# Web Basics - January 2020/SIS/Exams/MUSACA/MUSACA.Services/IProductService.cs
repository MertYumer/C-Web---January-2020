namespace MUSACA.Services
{
    using System.Collections.Generic;

    using MUSACA.Models;

    public interface IProductService
    {
        ICollection<Product> GetAllProducts();

        Product CreateProduct(Product product);
    }
}
