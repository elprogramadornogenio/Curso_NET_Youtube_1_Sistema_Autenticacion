using System.ComponentModel.DataAnnotations;

namespace autenticacionApp.DTOs
{
    public class RegistroDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(8, MinimumLength =4)]
        public string Password { get; set; }
    }
}