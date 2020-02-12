namespace IRunes.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Track
    {
        public Track()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        [Range(typeof(decimal), "0", "79228162514264337593543950335")]
        public decimal Price { get; set; }

        public string AlbumId { get; set; }
        public Album Album { get; set; }
    }
}
