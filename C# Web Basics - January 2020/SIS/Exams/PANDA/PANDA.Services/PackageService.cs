namespace PANDA.Services
{
    using PANDA.Data;
    using PANDA.Models;
    using SIS.MvcFramework.Mapping;
    using System.Linq;

    public class PackageService : IPackageService
    {
        private readonly PandaDbContext context;

        public PackageService(PandaDbContext context)
        {
            this.context = context;
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

            package = context.Packages.Add(package).Entity;
            context.SaveChanges();

            return true;
        }

        public IQueryable<Package> GetAllByStatus(PackageStatus status)
        {
            var packages = this.context
                .Packages
                .Where(x => x.Status == status);

            return packages;
        }
    }
}
