namespace hotel_manager.Entities
{
    public class Client
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
    }
}