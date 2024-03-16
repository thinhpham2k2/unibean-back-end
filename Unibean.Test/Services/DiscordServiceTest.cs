using FluentAssertions;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class DiscordServiceTest
{
    [Fact]
    public void DiscordService_ValidateUrl_ReturnFalse()
    {
        // Arrange
        string url = "url";

        // Act
        var result = DiscordService.ValidateUrl(url);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void DiscordService_ValidateUrl_ReturnTrue()
    {
        // Arrange
        string url = "https://url.com";

        // Act
        var result = DiscordService.ValidateUrl(url);

        // Assert
        Assert.True(result);
    }
}
