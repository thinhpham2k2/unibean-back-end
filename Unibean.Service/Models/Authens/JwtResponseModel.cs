namespace Unibean.Service.Models.Authens;

public class JwtResponseModel
{
    public string Jwt { get; set; }
    public object User { get; set; }
    public string Role { get; set; }
}
