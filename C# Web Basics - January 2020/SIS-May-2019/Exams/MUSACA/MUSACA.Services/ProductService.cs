namespace MUSACA.Services
{
    using System.Collections.Generic;
    using System.Linq;

    using MUSACA.Data;
    using MUSACA.Models;

    public class ProductService : IProductService
    {
        private readonly MusacaDbContext context;

        public ProductService(MusacaDbContext context)
        {
            this.context = context;
        }

        public Product CreateProduct(Product product)
        {
            product = this.context.Products.Add(product).Entity;
            this.context.SaveChanges();

            return product;
        }

        public ICollection<Product> GetAllProducts()
        {
            var products = this.context.Products.ToList();
            return products;
        }

        public Product GetProductByName(string productName)
        {
            var product = this.context
                .Products
                .SingleOrDefault(p => p.Name == productName);

            return product;
        }
    }
}
