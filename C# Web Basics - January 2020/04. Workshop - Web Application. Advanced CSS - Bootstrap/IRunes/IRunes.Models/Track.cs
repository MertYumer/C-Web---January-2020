namespace IRunes.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Track
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(50), MinLength(3)]
        public string Name { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        [Range(0, 1000)]
        public decimal Price { get; set; }

        public string AlbumId { get; set; }
        public Album Album { get; set; }
    }
}
