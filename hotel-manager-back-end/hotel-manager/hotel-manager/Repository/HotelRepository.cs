using hotel_manager.Data;
using hotel_manager.Entities;
using Microsoft.EntityFrameworkCore;

namespace hotel_manager.Repository
{
    public class HotelRepository
    {
        private readonly HotelDbContext _context;

        public HotelRepository(HotelDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            Hotel hotel,
            CancellationToken cancellationToken)
        {
            _context.Hotels.Add(hotel);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}