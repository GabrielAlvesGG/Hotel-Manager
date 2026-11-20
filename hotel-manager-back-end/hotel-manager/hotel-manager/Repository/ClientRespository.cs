using hotel_manager.Data;
using hotel_manager.Entities;
using hotel_manager.Mapping;
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

        public async Task<ClientListResponse> ListAsync(
            string? search,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            const string searchCollation = "Latin1_General_100_CI_AS";
            var clients = _context.Clients.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                clients = clients.Where(client =>
                    EF.Functions.Collate(client.Name, searchCollation).Contains(search) ||
                    EF.Functions.Collate(client.Email, searchCollation).Contains(search));
            }

            var totalCount = await clients.CountAsync(cancellationToken);
            var items = await clients
                .OrderBy(client => client.Name)
                .ThenBy(client => client.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(client => new ClientListItem(client.Name, client.Email))
                .ToListAsync(cancellationToken);

            return new ClientListResponse(items, totalCount, page, pageSize);
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
        public async Task<bool> ExistsByIdAsync(
int id,
CancellationToken cancellationToken)
        {
            return await _context.Clients.AnyAsync(
                client => client.Id == id,
                cancellationToken);
        }
    }
}