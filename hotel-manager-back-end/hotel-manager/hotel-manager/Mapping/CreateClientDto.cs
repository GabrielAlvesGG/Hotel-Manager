using System.ComponentModel.DataAnnotations;

namespace hotel_manager.Mapping
{
    public class CreateClientDto
    {
        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(254)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(128, MinimumLength = 15)]
        public string Password { get; set; } = string.Empty;
    }
}