namespace PANDA.Services
{
    using System.Linq;

    using PANDA.Models;

    public interface IReceiptService
    {
        void CreateFromPackage(string packageId, string recipientId, decimal weight);

        IQueryable<Receipt> GetAll();
    }
}
