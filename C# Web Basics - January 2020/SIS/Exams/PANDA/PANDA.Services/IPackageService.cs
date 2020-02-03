namespace PANDA.Services
{
    using System.Linq;


    using PANDA.Models;

    public interface IPackageService
    {
        bool CreatePackage(string description, decimal weight, string shippingAddress, string recipientName);

        IQueryable<Package> GetAllByStatus(PackageStatus status);
    }
}
