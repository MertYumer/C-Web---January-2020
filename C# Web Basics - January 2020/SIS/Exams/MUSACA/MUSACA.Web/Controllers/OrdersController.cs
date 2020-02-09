namespace MUSACA.Web.Controllers
{
    using MUSACA.Services;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.Result;

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
            var orderId = this.orderService
                .GetActiveOrderByCashierId(this.User.Id)
                .Id;

            this.orderService.CompleteOrder(orderId, this.User.Id);

            return this.Redirect("/");
        }
    }
}
