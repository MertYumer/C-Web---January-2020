namespace MUSACA.Web.Controllers
{
    using MUSACA.Services;
    using MUSACA.Web.ViewModels.Orders;
    using MUSACA.Web.ViewModels.Products;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.Mapping;
    using SIS.MvcFramework.Result;
    using System.Linq;

    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [Authorize]
        public IActionResult Cashout()
        {
            var order = this.orderService.GetActiveOrderByCashierId(this.User.Id);

            var orderHomeViewModel = new OrderHomeViewModel();
            orderHomeViewModel.Products = order
                .Products
                .Select(ModelMapper.ProjectTo<ProductHomeViewModel>)
                .ToList();

            this.orderService.CompleteOrder(order.Id, this.User.Id);

            return this.Redirect("/");
        }
    }
}
