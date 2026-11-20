using hotel_manager.Entities;
using hotel_manager.Mapping;
using hotel_manager.Repository;

namespace hotel_manager.Service
{
    public class HotelService
    {
        private readonly HotelRepository _repository;
        private readonly ClientRepository _clientRepository;

        public HotelService(
            HotelRepository repository,
            ClientRepository clientRepository)
        {
            _repository = repository;
            _clientRepository = clientRepository;
        }

        public async Task<int> RegisterAsync(
            ClientRegisterDto dto,
            int clientId,
            CancellationToken cancellationToken)
        {
            var clientExists = await _clientRepository.ExistsByIdAsync(
                clientId,
                cancellationToken);

            if (!clientExists)
            {
                throw new UnauthorizedAccessException(
                    "Cliente não encontrado. Entre novamente na aplicação.");
            }

            var hotel = new Hotel
            {
                Cnpj = dto.CNPJ.Trim(),
                Name = dto.name.Trim(),
                Localization = dto.localization.Trim(),
                HotelType = dto.hotelType.Trim(),
                ClientId = clientId
            };

            await _repository.AddAsync(hotel, cancellationToken);

            return hotel.Id;
        }
    }
}