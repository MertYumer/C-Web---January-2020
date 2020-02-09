namespace MUSACA.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class OrderProduct
    {
        [Required]
        [ForeignKey("Order")]
        public string OrderId { get; set; }

        public Order Order { get; set; }

        [Required]
        [ForeignKey("Product")]
        public string ProductId { get; set; }

        public Product Product { get; set; }
    }
}
