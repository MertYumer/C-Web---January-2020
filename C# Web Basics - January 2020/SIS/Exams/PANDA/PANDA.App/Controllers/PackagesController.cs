namespace PANDA.Web.Controllers
{
    using PANDA.Models;
    using PANDA.Services;
    using PANDA.Web.ViewModels.Packages;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Attributes.Http;
    using SIS.MvcFramework.Attributes.Security;
    using SIS.MvcFramework.Mapping;
    using SIS.MvcFramework.Result;
    using System.Linq;

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
            var modelPackages = this.packageService
                .GetAllByStatus(PackageStatus.Pending)
                .Select(ModelMapper.ProjectTo<PackageViewModel>)
                .ToList();

            return this.View(new PackagesListViewModel { Packages = modelPackages });
        }
    }
}
