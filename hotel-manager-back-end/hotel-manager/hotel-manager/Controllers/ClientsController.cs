using hotel_manager.Mapping;
using hotel_manager.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hotel_manager.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly ClientService _service;

        public ClientsController(ClientService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ClientListResponse>> List(
            CancellationToken cancellationToken,
            [FromQuery] string? search = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (page < 1 || pageSize is < 1 or > 100 ||
                (page - 1L) * pageSize > int.MaxValue)
            {
                return BadRequest(new { message = "Paginação inválida." });
            }

            return await _service.ListAsync(search?.Trim(), page, pageSize, cancellationToken);
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            [FromBody] CreateClientDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var id = await _service.RegisterAsync(
                    dto,
                    cancellationToken);

                return StatusCode(StatusCodes.Status201Created, new
                {
                    id,
                    message = "Cliente cadastrado com sucesso."
                });
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new
                {
                    message = exception.Message
                });
            }
            catch (InvalidOperationException exception)
            {
                return Conflict(new
                {
                    message = exception.Message
                });
            }
        }
    }
}