namespace PANDA.Services
{
    using System;
    using System.Linq;

    using PANDA.Data;
    using PANDA.Models;

    public class ReceiptService : IReceiptService
    {
        private readonly PandaDbContext context;

        public ReceiptService(PandaDbContext context)
        {
            this.context = context;
        }

        public void CreateFromPackage(string packageId, string recipientId, decimal weight)
        {
            var receipt = new Receipt
            { 
                Fee = weight * 2.67m,
                IssuedOn = DateTime.UtcNow,
                PackageId = packageId,
                RecipientId = recipientId
            };

            this.context.Receipts.Add(receipt);
            this.context.SaveChanges();
        }

        public IQueryable<Receipt> GetAll()
        {
            return this.context.Receipts;
        }
    }
}
