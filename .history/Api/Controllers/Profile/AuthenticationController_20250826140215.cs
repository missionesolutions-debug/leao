using Framework.Domain.Dtos.Common;
using Framework.Domain.Dtos.Profile;
using Framework.Repositories.Factories.Subscription;
using Framework.Services.ApplicationServices.Profile;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;

namespace Api.Controllers.Profile
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly AuthenticationService _authenticationService;

        public AuthenticationController(ILogger<AuthenticationController> logger, AuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(LoginResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UsuarioLoginDto usuarioLoginDTO)
        {
            // Validate the model
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.ToDictionary(
                    key => key.Key,
                    value => new ErrorKeyString { Errors = value.Value.Errors.Select(e => e.ErrorMessage).ToList() }
                );

                var errorResult = new ErrorResult(HttpStatusCode.BadRequest, validationErrors);
                return BadRequest(errorResult);
            }

            try
            {
                var result = await _authenticationService.Login(usuarioLoginDTO);
                return Ok(result);
            }
            catch (AuthenticationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Erro ao processar a solicitação de login." });
            }
        }



        [HttpGet("/Users/me")]
        public IActionResult GetAuthenticatedUser()
        {
            try
            {
                // Find the email claim in the user's claims
                var emailAddress = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                // Obtém o e-mail do usuário a partir do token JWT
                if (string.IsNullOrEmpty(emailAddress))
                {
                    return Unauthorized(new { message = "Token inválido ou ausente." });
                }

                // Chama o service para obter os dados do usuário
                var userAdmin = _authenticationService.GetAuthenticatedUser(emailAddress);
                return Ok(userAdmin);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter o usuário autenticado.");
                return StatusCode(500, new { message = "Erro ao processar a solicitação." });
            }
        }
    }
}