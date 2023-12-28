using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Unibean.Service.Models.Authens;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class JwtService : IJwtService
{
    private readonly Mapper mapper;

    public JwtService()
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<IEnumerable<Claim>, JwtRequestModel>()
            .ForMember(a => a.AccountId, opt 
            => opt.MapFrom(src => src.Where(c => c.Type.Equals("sid")).FirstOrDefault().Value))
            .ForMember(a => a.UserId, opt 
            => opt.MapFrom(src => src.Where(c => c.Type.Equals("userid")).FirstOrDefault().Value))
            .ForMember(a => a.Role, opt
            => opt.MapFrom(src => src.Where(c => c.Type.Equals("role")).FirstOrDefault().Value))
            .ReverseMap();
        });
        mapper = new Mapper(config);
    }

    public JwtRequestModel GetJwtRequest(string jwtToken)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var decodedToken = tokenHandler.ReadJwtToken(jwtToken);

        return mapper.Map<JwtRequestModel>(decodedToken.Claims);
    }
}
