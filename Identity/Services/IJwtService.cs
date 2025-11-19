using Infrastructure;

namespace Identity.Services
{
    public interface IJwtService
    {
        string GenerateToken(ApplicationUser user);
    }
}
