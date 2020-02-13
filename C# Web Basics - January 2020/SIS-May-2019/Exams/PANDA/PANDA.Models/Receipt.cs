namespace PANDA.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Receipt
    {
        public Receipt()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        public decimal Fee { get; set; }

        public DateTime IssuedOn { get; set; }

        [Required]
        [ForeignKey("Recipient")]
        public string RecipientId { get; set; }

        public virtual User Recipient { get; set; }

        [Required]
        [ForeignKey("Package")]
        public string PackageId { get; set; }

        public virtual Package Package { get; set; }
    }
}
