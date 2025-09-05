using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Framework.Services.Interfaces.Profile
{
    public interface IAccountService
    {
        Task<List<Claim>> GetUserClaimsAsync(int id);
        List<Claim> GetUserClaims(int id);
    }
}
