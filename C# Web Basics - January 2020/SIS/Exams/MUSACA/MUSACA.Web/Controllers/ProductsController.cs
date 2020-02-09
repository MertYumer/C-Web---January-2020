namespace MUSACA.Web.Controllers
{
    using System.Linq;

    using MUSACA.Models;
    using MUSACA.Services;
    using MUSACA.Web.BindingModels.Products;
    using MUSACA.Web.ViewModels.Products;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Http;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.Mapping;
    using SIS.MvcFramework.Result;

    public class ProductsController : Controller
    {
        private readonly IProductService productService;
        private readonly IOrderService orderService;

        public ProductsController(IProductService productService, IOrderService orderService)
        {
            this.productService = productService;
            this.orderService = orderService;
        }

        [Authorize]
        public IActionResult All()
        {
            var products = this.productService
                .GetAllProducts()
                .To<ProductAllViewModel>()
                .ToList();

            return this.View(products);
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(ProductCreateBindingModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Products/Create");
            }

            var product = ModelMapper.ProjectTo<Product>(model);
            this.productService.CreateProduct(product);

            return this.Redirect("/Products/All");
        }

        [Authorize]
        [HttpPost]
        public IActionResult Order(ProductOrderBindingModel productOrderBindingModel)
        {
            Product productToOrder = this.productService.GetProductByName(productOrderBindingModel.Product);

            if (productToOrder != null)
            {
                this.orderService.AddProductToCurrentActiveOrder(productToOrder.Id, this.User.Id);
            }

            return this.Redirect("/");
        }
    }
}
