namespace MUSACA.Web.Controllers
{
    using MUSACA.Services;
    using MUSACA.Web.ViewModels.Orders;
    using MUSACA.Web.ViewModels.Products;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Http;
    using SIS.MvcFramework.Mapping;
    using SIS.MvcFramework.Result;

    public class HomeController : Controller
    {
        private readonly IOrderService orderService;

        public HomeController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpGet(Url = "/")]
        public IActionResult IndexSlash()
        {
            return Index();
        }

        public IActionResult Index()
        {
            var orderHomeViewModel = new OrderHomeViewModel();

            if (this.IsLoggedIn())
            {
                var activeOrder = this.orderService
                    .GetActiveOrderByCashierId(User.Id);

                orderHomeViewModel = activeOrder.To<OrderHomeViewModel>();

                orderHomeViewModel.Products.Clear();

                foreach (var orderProduct in activeOrder.Products)
                {
                    ProductHomeViewModel productHomeViewModel = orderProduct
                        .Product
                        .To<ProductHomeViewModel>();

                    orderHomeViewModel.Products.Add(productHomeViewModel);
                }
            }

            return this.View(orderHomeViewModel);
        }
    }
}
