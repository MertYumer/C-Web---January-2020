namespace MUSACA.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Order
    {
        public Order()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Status = OrderStatus.Active;
            this.Products = new HashSet<OrderProduct>();
        }

        public string Id { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime IssuedOn { get; set; }

        public ICollection<OrderProduct> Products { get; set; }

        [Required]
        [ForeignKey("Cashier")]
        public string CashierId { get; set; }

        public User Cashier { get; set; }
    }
}
