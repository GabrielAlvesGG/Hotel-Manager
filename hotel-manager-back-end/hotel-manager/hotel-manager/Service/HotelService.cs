using hotel_manager.Entities;
using hotel_manager.Mapping;
using hotel_manager.Repository;

namespace hotel_manager.Service
{
    public class HotelService
    {
        private readonly HotelRepository _repository;

        public HotelService(HotelRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> RegisterAsync(
            ClientRegisterDto dto,
            CancellationToken cancellationToken)
        {
            var hotel = new Hotel
            {
                Cnpj = dto.CNPJ.Trim(),
                Name = dto.name.Trim(),
                Localization = dto.localization.Trim(),
                HotelType = dto.hotelType.Trim()
            };

            await _repository.AddAsync(hotel, cancellationToken);

            return hotel.Id;
        }
    }
}