using hotel_manager.Mapping;
using hotel_manager.Service;
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