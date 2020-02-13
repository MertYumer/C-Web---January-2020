namespace PANDA.Web.Controllers
{
    using System.Linq;

    using PANDA.Models;
    using PANDA.Services;
    using PANDA.Web.ViewModels.Packages;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Http;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.Result;

    public class PackagesController : Controller
    {
        private readonly IPackageService packageService;
        private readonly IUserService userService;

        public PackagesController(IPackageService packageService, IUserService userService)
        {
            this.packageService = packageService;
            this.userService = userService;
        }

        [Authorize]
        public IActionResult Create()
        {
            var list = this.userService.GetUsernames();

            return this.View(list);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(PackageCreateInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect("/Package/Create");
            }

            var successfullyAdded = this.packageService
                .CreatePackage(model.Description, model.Weight, model.ShippingAddress, model.RecipientName);

            if (!successfullyAdded)
            {
                return this.Redirect("/Package/Create");
            }

            return this.Redirect("/Packages/Pending");
        }

        [Authorize]
        public IActionResult Pending()
        {
            var packages = this.packageService.GetAllByStatus(PackageStatus.Pending)
                .Select(x => new PackageViewModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    Weight = x.Weight,
                    ShippingAddress = x.ShippingAddress,
                    RecipientName = x.Recipient.Username,
                })
                .ToList();

            return this.View(new PackagesListViewModel { Packages = packages });
        }

        [Authorize]
        public IActionResult Delivered()
        {
            var packages = this.packageService.GetAllByStatus(PackageStatus.Delivered)
                .Select(x => new PackageViewModel
                {
                    Description = x.Description,
                    Id = x.Id,
                    Weight = x.Weight,
                    ShippingAddress = x.ShippingAddress,
                    RecipientName = x.Recipient.Username,
                })
                .ToList();

            return this.View(new PackagesListViewModel { Packages = packages });
        }

        [Authorize]
        public IActionResult Deliver(string id)
        {
            this.packageService.Deliver(id);

            return this.Redirect("/Packages/Delivered");
        }
    }
}
