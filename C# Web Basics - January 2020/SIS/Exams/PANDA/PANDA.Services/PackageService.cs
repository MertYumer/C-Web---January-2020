namespace PANDA.Services
{
    using System.Linq;

    using PANDA.Data;
    using PANDA.Models;

    public class PackageService : IPackageService
    {
        private readonly PandaDbContext context;
        private readonly IReceiptService receiptService;

        public PackageService(PandaDbContext context, IReceiptService receiptService)
        {
            this.context = context;
            this.receiptService = receiptService;
        }

        public bool CreatePackage(string description, decimal weight, string shippingAddress, string recipientName)
        {
            var userId = this.context
                .Users
                .Where(x => x.Username == recipientName)
                .Select(x => x.Id)
                .FirstOrDefault();

            if (userId == null)
            {
                return false;
            }

            var package = new Package
            {
                Description = description,
                Weight = weight,
                Status = PackageStatus.Pending,
                ShippingAddress = shippingAddress,
                RecipientId = userId,
            };

            this.context.Packages.Add(package);
            this.context.SaveChanges();

            return true;
        }

        public IQueryable<Package> GetAllByStatus(PackageStatus status)
        {
            var packages = this.context
                .Packages
                .Where(x => x.Status == status);

            return packages;
        }

        public void Deliver(string id)
        {
            var package = this.context
                .Packages
                .FirstOrDefault(p => p.Id == id);

            if (package == null)
            {
                return;
            }

            package.Status = PackageStatus.Delivered;
            this.context.SaveChanges();

            this.receiptService.CreateFromPackage(package.Id, package.RecipientId, package.Weight);
        }
    }
}
