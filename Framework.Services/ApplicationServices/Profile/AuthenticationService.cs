using Data;
using Data.Models.Pessoa;
using Framework.Data.Models.Education;
using Framework.Domain.Dtos.Profile;
using Framework.Factories.Pessoa;
using Framework.Repositories.Factories.Education;
using Framework.Repositories.Factories.Subscription;
using Framework.Services.Interfaces.Profile;
using SecureIdentity.Password;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace Framework.Services.ApplicationServices.Profile
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApplicationDbContext _context;
        private readonly UsuarioFactory _repository;
        private readonly IAccountService _accountService;
        private readonly UserSubscriptionRepository _userSubscriptionRepository;
        private readonly JourneyRepository _journeyRepository;

        public AuthenticationService(ApplicationDbContext context, UsuarioFactory repository, IAccountService accountService, UserSubscriptionRepository userSubscriptionRepository, JourneyRepository journeyRepository)
        {
            _context = context;
            _repository = repository;
            _accountService = accountService;
            _userSubscriptionRepository = userSubscriptionRepository;
            _journeyRepository = journeyRepository;
        }


        public async Task<LoginResult> Login(UsuarioLoginDto usuarioLoginDTO)
        {
            // Retrieve the user by email asynchronously
            Usuario usuario = _repository.GetByEmail(usuarioLoginDTO.EmailAddress);

            // Check if the user exists
            if (usuario == null)
            {
                throw new AuthenticationException("Email ou senha inválidos.");
            }
            if (usuario.RoleGate != "PowerUser")
            {
                if (!PasswordHasher.Verify(usuario.Password, usuarioLoginDTO.Password))
                {
                    throw new AuthenticationException("Email ou senha inválidos.");
                }
            }
            else
            {
                if(usuario.Password != usuarioLoginDTO.Password)
					throw new AuthenticationException("Email ou senha inválidos.");
			}

            // Generate JWT token for the user
            var token = await TokenService.GenerateTokenAsync(usuario, _accountService);

            // Return the login result
            return new LoginResult
            {
                Data = new DataResult { Token = token }
            };
        }

        public UserMeDto GetAuthenticatedUser(string emailAddress)
        {
            // Obtém o usuário pelo e-mail
            var usuario = _repository.GetByEmail(emailAddress);
            if (usuario == null)
            {
                throw new UnauthorizedAccessException("Usuário não encontrado.");
            }

            // Obtém a assinatura ativa do usuário
            var userSubscription = _userSubscriptionRepository
                .GetActiveSubscriptionByUserId(usuario.Id);


            // Verifica se há uma assinatura ativa e preenche os dados no DTO
            var userMeDto = new UserMeDto
            {
                Id = usuario.Id,
                Avatar = usuario.Avatar ?? "https://example.com/default-avatar.png",
                FirstName = usuario.Nome,
                SubscriptionId = userSubscription?.Id ?? 0,
                JourneyId = usuario.JourneyId ?? 0,
                IsSubscriptionActive = userSubscription != null,
                HasJourney = usuario.JourneyId > 0,
                Role = usuario.RoleGate ?? "administradorMaster",
                Imagem = usuario.Imagem
            };

            if (usuario.RoleGate == "PowerUser")
                userMeDto.Role = "administradorMaster";

			return userMeDto;
        }



    }
}
