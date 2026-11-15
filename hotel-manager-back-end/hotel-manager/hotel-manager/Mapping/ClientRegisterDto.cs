using System.ComponentModel.DataAnnotations;

namespace hotel_manager.Mapping
{
    public class ClientRegisterDto
    {
        [Required]
        [StringLength(20)]
        public string CNPJ { get; set; } = string.Empty;

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string name { get; set; } = string.Empty;

        [Required]
        [StringLength(300)]
        public string localization { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string hotelType { get; set; } = string.Empty;
    }
}