using Data;
using Data.Models.Pessoa;
using Framework.Data.Models.Users;
using Framework.Domain.Dtos.Common;
using Framework.Domain.Dtos.Profile;
using Framework.Domain.Exceptions;
using Framework.Factories.Pessoa;
using Framework.Infrastructure.Mailing.MessageProvider;
using Framework.Repositories.Interface.Account;
using Framework.Services.Interfaces.Profile;
using Framework.Services.Mapper.Profile;
using SecureIdentity.Password;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Framework.Services.ApplicationServices.Profile
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly UsuarioFactory _repository;
        private readonly IUserClaimRepository _userClaimRepository;
        private readonly IEmailService _emailService;

        public AccountService(ApplicationDbContext context, UsuarioFactory repository, IUserClaimRepository userClaimRepository, IEmailService emailService)
        {
            _context = context;
            _repository = repository;
            _userClaimRepository = userClaimRepository;
            _emailService = emailService;
        }

        public async Task<dynamic> CreateAccount(CreateAccountResponse model)
        {
            Usuario usuario =  _repository.GetUser(model.Email, model.CPF);

            if (usuario is not null && usuario.Id > 0) throw new UserAlreadyExistsException("Usuário já existe");

            model.Password = PasswordHasher.Hash(model.Password);

            usuario = AccountMapper.MappingUser(model);

            await _repository.AddUsuarioAsync(usuario);

            var token = await TokenService.GenerateTokenAsync(usuario, this);

            return new CreateAccountResult (){ Data = new UserDto() { Token = token }, Message = "Usuário criado com sucesso" };
        }

        public async Task<UpdateAccountResult> UpdateAccount(UpdateAccountResponse model)
        {
            // Retrieve the user by email
            var usuario = await _repository.GetUserAsync(model.Email);

            if (usuario == null)
            {
                throw new UserNotFoundException("Usuário não encontrado.");
            }

            // Update user properties only if they are provided
            if (!string.IsNullOrEmpty(model.Nome))
            {
                usuario.Nome = model.Nome;
            }
          
            await _repository.UpdateObjAsync(usuario);

            return new UpdateAccountResult
            {
                Success = true,
                Message = "Conta atualizada com sucesso.",
            };
        }

        public async Task<dynamic> Verification(string email, string token)
        {
            throw new NotImplementedException();
        }

        #region Password
        public async Task ForgotPasswordAsync(string email)
        {
            var user = await _repository.GetUserAsync(email);
            if (user == null)
            {
                throw new Exception("User with the specified email does not exist.");
            }

            user.PasswordTokenExpiry = DateTime.UtcNow;
            user.PasswordToken = Guid.NewGuid().ToString();

            await _emailService.SendPasswordResetEmailAsync(user.Email, user.PasswordToken,"",user.Nome,"");
        }

        public async Task<string> CheckTokenReturnEmailAsync(string token)
        {
            var user = await _repository.GetByPasswordResetGuidAsync(token);
            if (user == null)
            {
                throw new Exception("Invalid or expired token.");
            }
            return user.Email;
        }

        public async Task CheckIfPasswordEqualsThenEditAsync(ChangePasswordResponse model, string userEmail)
        {
            var user = await _repository.GetUserAsync(userEmail);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (!PasswordHasher.Verify(user.Password, model.CurrentPassword))
            {
                throw new Exception("Current password is incorrect.");
            }

            user.Password = PasswordHasher.Hash(model.NewPassword);

            await _repository.UpdateObjAsync(user);
        }

        public async Task CheckIfPasswordEqualsAndGuidAsync(ChangePasswordByForgotResponse model)
        {
            var user = await _repository.GetByPasswordResetGuidAsync(model.Guid);
            if (user == null)
            {
                throw new Exception("Invalid or expired password reset link.");
            }

            if (PasswordHasher.Verify(user.Password, model.NewPassword))
            {
                throw new Exception("The new password must be different from the current password.");
            }

            user.Password = PasswordHasher.Hash(model.NewPassword);

            await _repository.UpdateObjAsync(user);
        }
        #endregion

        #region Claims
        public async Task AddClaimToUserAsync(int usuarioId, string claimType, string claimValue)
        {
            var existingClaim = await _userClaimRepository.GetUserClaimAsync(usuarioId, claimType);
            if (existingClaim != null)
            {
                // Update the claim value
                existingClaim.ClaimValue = claimValue;
                await _userClaimRepository.AddClaimAsync(existingClaim);
            }
            else
            {
                // Add a new claim
                var userClaim = new UserClaim
                {
                    UsuarioId = usuarioId,
                    ClaimType = claimType,
                    ClaimValue = claimValue
                };
                await _userClaimRepository.AddClaimAsync(userClaim);
            }
        }

        public async Task RemoveClaimFromUserAsync(int usuarioId, string claimType)
        {
            var userClaim = await _userClaimRepository.GetUserClaimAsync(usuarioId, claimType);
            if (userClaim != null)
            {
                await _userClaimRepository.RemoveClaimAsync(userClaim);
            }
        }

        public async Task<List<Claim>> GetUserClaimsAsync(int usuarioId)
        {
            var userClaims = await _userClaimRepository.GetUserClaimsAsync(usuarioId);
            return userClaims.Select(uc => new Claim(uc.ClaimType, uc.ClaimValue)).ToList();
        }

        public List<Claim> GetUserClaims(int usuarioId)
        {
            var userClaims = _userClaimRepository.GetUserClaims(usuarioId);
            return userClaims.Select(uc => new Claim(uc.ClaimType, uc.ClaimValue)).ToList();
        }

        public Usuario GetUserByEmail(string emailAddress)
        {
            return _repository.GetByEmail(emailAddress);
        }

        public object EncryptPassword(string password)
        {
            return new { hash = PasswordHasher.Hash(password) };
        }

        #endregion
    }
}
