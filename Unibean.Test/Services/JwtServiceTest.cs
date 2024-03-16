using FluentAssertions;
using Unibean.Service.Models.Authens;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class JwtServiceTest
{
    [Fact]
    public void JwtService_GetJwtRequest()
    {
        // Arrange
        string jwtToken = null;
        var service = new JwtService();

        // Act
        var result = service.GetJwtRequest(jwtToken);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(JwtRequestModel));
        Assert.Equal("User", result.Role);
    }
}
