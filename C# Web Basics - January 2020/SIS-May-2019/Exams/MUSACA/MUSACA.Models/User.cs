namespace MUSACA.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [Required]
        [MinLength(5), MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MinLength(5), MaxLength(20)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
