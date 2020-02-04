namespace PANDA.Web.Controllers
{
    using System.Linq;

    using PANDA.Services;
    using PANDA.Web.ViewModels.Receipts;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.Result;

    public class ReceiptsController : Controller
    {
        private readonly IReceiptService receiptService;

        public ReceiptsController(IReceiptService receiptService)
        {
            this.receiptService = receiptService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var receipts = this.receiptService
                .GetAll()
                .Select(x => new ReceiptViewModel
                { 
                    Id = x.Id,
                    Fee = x.Fee,
                    IssuedOn = x.IssuedOn,
                    RecipientName = x.Recipient.Username
                })
                .ToList();

            return this.View(receipts);
        }
    }
}
