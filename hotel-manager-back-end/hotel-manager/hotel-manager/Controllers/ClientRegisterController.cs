using hotel_manager.Mapping;
using hotel_manager.Service;
using Microsoft.AspNetCore.Mvc;

namespace hotel_manager.Controllers
{
    [ApiController]
    [Route("api/client-register")]
    public class ClientRegisterController : ControllerBase
    {
        private readonly HotelService _service;

        public ClientRegisterController(HotelService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterClient(
            [FromBody] ClientRegisterDto clientRegisterDto,
            CancellationToken cancellationToken)
        {
            var id = await _service.RegisterAsync(
                clientRegisterDto,
                cancellationToken);

            return StatusCode(StatusCodes.Status201Created, new
            {
                id,
                message = "Hotel cadastrado com sucesso."
            });
        }
    }
}