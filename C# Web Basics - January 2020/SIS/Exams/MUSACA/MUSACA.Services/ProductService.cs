namespace MUSACA.Services
{
    using System;
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
    }
}
