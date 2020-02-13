namespace MUSACA.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using MUSACA.Data;
    using MUSACA.Models;

    public class OrderService : IOrderService
    {
        private readonly MusacaDbContext context;

        public OrderService(MusacaDbContext context)
        {
            this.context = context;
        }

        public ICollection<Order> GetAllCompletedOrdersByCashierId(string userId)
        {
            var orders = this.context
                .Orders
                .Include(o => o.Products)
                .ThenInclude(op => op.Product)
                .Include(o => o.Cashier)
                .Where(o => o.CashierId == userId)
                .Where(o => o.Status == OrderStatus.Completed)
                .ToList();

            return orders;
        }

        public Order GetActiveOrderByCashierId(string userId)
        {
            var order = context
                .Orders
                .Include(o => o.Products)
                .ThenInclude(op => op.Product)
                .Include(o => o.Cashier)
                .SingleOrDefault(o => o.CashierId == userId && o.Status == OrderStatus.Active);

            return order;
        }

        public Order CompleteOrder(string orderId, string userId)
        {
            Order orderFromDb = this.context.Orders.SingleOrDefault(order => order.Id == orderId);

            orderFromDb.IssuedOn = DateTime.UtcNow;
            orderFromDb.Status = OrderStatus.Completed;

            this.context.Update(orderFromDb);
            this.context.SaveChanges();

            this.CreateOrder(new Order { CashierId = userId });

            return orderFromDb;
        }

        public bool AddProductToCurrentActiveOrder(string productId, string userId)
        {
            var productFromDb = this.context
                .Products
                .SingleOrDefault(product => product.Id == productId);

            var activeOrder = this.GetActiveOrderByCashierId(userId);
            activeOrder.Products.Add(new OrderProduct
            {
                Product = productFromDb
            });

            this.context.Update(activeOrder);
            this.context.SaveChanges();

            return true;
        }

        public Order CreateOrder(Order order)
        {
            this.context.Add(order);
            this.context.SaveChanges();

            return order;
        }
    }
}
