using Microsoft.AspNetCore.Identity;
namespace ThucHanhAPI2.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
