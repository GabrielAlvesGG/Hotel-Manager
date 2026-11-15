using System.ComponentModel.DataAnnotations;

namespace hotel_manager.Mapping
{
    public class LoginClientDto
    {
        [Required]
        [EmailAddress]
        [StringLength(254)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(128)]
        public string Password { get; set; } = string.Empty;
    }
}