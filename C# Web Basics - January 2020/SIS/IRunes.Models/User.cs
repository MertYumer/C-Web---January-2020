namespace IRunes.Models
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        [Key]
        public string Id { get; set; }

        [Required]
        [MaxLength(20), MinLength(3)]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string Email { get; set; }
    }
}
