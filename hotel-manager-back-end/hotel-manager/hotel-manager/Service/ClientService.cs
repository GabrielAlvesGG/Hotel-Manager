using hotel_manager.Entities;
using hotel_manager.Mapping;
using hotel_manager.Repository;
using Microsoft.AspNetCore.Identity;

namespace hotel_manager.Service
{
    public class ClientService
    {
        private readonly ClientRepository _repository;
        private readonly IPasswordHasher<Client> _passwordHasher;

        public ClientService(
            ClientRepository repository,
            IPasswordHasher<Client> passwordHasher)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
        }

        public Task<ClientListResponse> ListAsync(
            string? search,
            int page,
            int pageSize,
            CancellationToken cancellationToken)
        {
            return _repository.ListAsync(search, page, pageSize, cancellationToken);
        }

        public async Task<int> RegisterAsync(
            CreateClientDto dto,
            CancellationToken cancellationToken)
        {
            var name = dto.Name.Trim();
            var email = dto.Email.Trim().ToLowerInvariant();

            if (name.Length < 3)
            {
                throw new ArgumentException(
                    "O nome deve ter pelo menos três caracteres.");
            }

            var emailExists = await _repository.EmailExistsAsync(
                email,
                cancellationToken);

            if (emailExists)
            {
                throw new InvalidOperationException(
                    "Já existe um cliente cadastrado com esse e-mail.");
            }

            var client = new Client
            {
                Name = name,
                Email = email
            };

            client.PasswordHash = _passwordHasher.HashPassword(
                client,
                dto.Password);

            await _repository.AddAsync(client, cancellationToken);

            return client.Id;
        }

        public async Task<Client?> ValidateCredentialsAsync(
            LoginClientDto dto,
            CancellationToken cancellationToken)
        {
            var email = dto.Email.Trim().ToLowerInvariant();

            var client = await _repository.GetByEmailAsync(
                email,
                cancellationToken);

            if (client is null)
            {
                return null;
            }

            var verification = _passwordHasher.VerifyHashedPassword(
                client,
                client.PasswordHash,
                dto.Password);

            if (verification == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return client;
        }
    }
}