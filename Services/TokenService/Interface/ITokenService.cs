using Core.IdentityEntities;

namespace Services.TokenService.Interface
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
