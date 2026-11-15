using hotel_manager.Data;
using hotel_manager.Entities;
using Microsoft.EntityFrameworkCore;

namespace hotel_manager.Repository
{
    public class ClientRepository
    {
        private readonly HotelDbContext _context;

        public ClientRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task<bool> EmailExistsAsync(
            string email,
            CancellationToken cancellationToken)
        {
            return await _context.Clients.AnyAsync(
                client => client.Email == email,
                cancellationToken);
        }

        public async Task<Client?> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken)
        {
            return await _context.Clients.SingleOrDefaultAsync(
                client => client.Email == email,
                cancellationToken);
        }

        public async Task AddAsync(
            Client client,
            CancellationToken cancellationToken)
        {
            _context.Clients.Add(client);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}