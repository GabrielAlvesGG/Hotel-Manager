namespace hotel_manager.Entities
{
    public class Hotel
    {
        public int Id { get; set; }

        public string Cnpj { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Localization { get; set; } = string.Empty;

        public string HotelType { get; set; } = string.Empty;

        public int ClientId { get; set; }

        public Client Client { get; set; } = null!;
    }
}