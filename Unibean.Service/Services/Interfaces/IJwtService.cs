using Unibean.Service.Models.Authens;

namespace Unibean.Service.Services.Interfaces;

public interface IJwtService
{
    JwtRequestModel GetJwtRequest(string jwtToken);
}
