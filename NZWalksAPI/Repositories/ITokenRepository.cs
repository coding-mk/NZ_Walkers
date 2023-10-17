using Microsoft.AspNetCore.Identity;

namespace NZWalksAPI.Repositories;

public interface ITokenRepository
{
  public string CreateJWTToken(IdentityUser user, List<string> roles);
}