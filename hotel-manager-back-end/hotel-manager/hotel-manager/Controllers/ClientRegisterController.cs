using System.Security.Claims;
using hotel_manager.Mapping;
using hotel_manager.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hotel_manager.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/client-register")]
    public class ClientRegisterController : ControllerBase
    {
        private readonly HotelService _service;

        public ClientRegisterController(HotelService service)
        {
            _service = service;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterClient(
            [FromBody] ClientRegisterDto clientRegisterDto,
            CancellationToken cancellationToken)
        {
            var identifier = User.FindFirstValue(
                ClaimTypes.NameIdentifier);

            if (!int.TryParse(identifier, out var clientId)
                || clientId <= 0)
            {
                return Unauthorized(new
                {
                    message = "Sessão inválida. Entre novamente."
                });
            }

            try
            {
                var id = await _service.RegisterAsync(
                    clientRegisterDto,
                    clientId,
                    cancellationToken);

                return StatusCode(StatusCodes.Status201Created, new
                {
                    id,
                    message = "Hotel cadastrado com sucesso."
                });
            }
            catch (UnauthorizedAccessException)
            {
                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);

                return Unauthorized(new
                {
                    message = "Cliente não encontrado. Entre novamente."
                });
            }
        }
    }
}