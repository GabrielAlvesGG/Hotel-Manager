using System.Globalization;
using System.Security.Claims;
using hotel_manager.Mapping;
using hotel_manager.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;

namespace hotel_manager.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ClientService _service;

        public AuthController(ClientService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginClientDto dto,
            CancellationToken cancellationToken)
        {
            var client = await _service.ValidateCredentialsAsync(
                dto,
                cancellationToken);

            if (client is null)
            {
                return Unauthorized(new
                {
                    message = "E-mail ou senha inválidos."
                });
            }

            var claims = new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    client.Id.ToString(CultureInfo.InvariantCulture)),

                new Claim(ClaimTypes.Name, client.Name),

                new Claim(ClaimTypes.Email, client.Email)
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = false
                });

            return Ok(new
            {
                id = client.Id,
                name = client.Name,
                email = client.Email,
                message = "Login realizado com sucesso."
            });
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                id = User.FindFirstValue(ClaimTypes.NameIdentifier),
                name = User.FindFirstValue(ClaimTypes.Name),
                email = User.FindFirstValue(ClaimTypes.Email)
            });
        }

        [AllowAnonymous]
        [HttpGet("csrf")]
        public IActionResult GetCsrfToken(
        [FromServices] IAntiforgery antiforgery)
        {
            var tokens = antiforgery.GetAndStoreTokens(HttpContext);

            Response.Headers.CacheControl = "no-store";

            return Ok(new
            {
                token = tokens.RequestToken
            });
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok(new
            {
                message = "Logout realizado com sucesso."
            });
        }
    }
}