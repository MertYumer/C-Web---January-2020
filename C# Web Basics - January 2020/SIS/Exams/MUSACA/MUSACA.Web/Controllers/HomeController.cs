namespace MUSACA.Web.Controllers
{
    using MUSACA.Models;
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



            return this.View(orderHomeViewModel);
        }
    }
}
