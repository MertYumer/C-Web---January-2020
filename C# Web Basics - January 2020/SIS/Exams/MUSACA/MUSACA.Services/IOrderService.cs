namespace MUSACA.Services
{
    using System.Collections.Generic;

    using MUSACA.Models;

    public interface IOrderService
    {
        ICollection<Order> GetAllCompletedOrdersByCashierId(string userId);

        Order GetActiveOrderByCashierId(string userId);

        Order CompleteOrder(string orderId, string userId);

        bool AddProductToCurrentActiveOrder(string productId, string userId);

        Order CreateOrder(Order order);
    }
}
