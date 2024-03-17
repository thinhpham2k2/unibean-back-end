using FluentAssertions;
using Unibean.Repository.Paging;
using Unibean.Service.Models.Admins;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class EmailServiceTest
{
    [Fact]
    public void EmailService_SendEmailBrandRegister_ReturnFalse()
    {
        // Arrange
        string receiver = "receiver";
        var service = new EmailService();

        // Act
        var result = service.SendEmailBrandRegister(receiver);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EmailService_SendEmailBrandRegister_ReturnTrue()
    {
        // Arrange
        string receiver = "receiver@gmail.com";
        var service = new EmailService();

        // Act
        var result = service.SendEmailBrandRegister(receiver);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void EmailService_SendEmailCamapaignClose()
    {
        // Arrange
        List<string> receivers = new()
        {
            "receiver",
            "receiver@gmail.com"
        };
        string campaignName = "campaignName";
        var service = new EmailService();

        // Act
        var result = service.SendEmailCamapaignClose(receivers, campaignName);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void EmailService_SendEmailStudentRegister_ReturnFalse()
    {
        // Arrange
        string receiver = "receiver";
        var service = new EmailService();

        // Act
        var result = service.SendEmailStudentRegister(receiver);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EmailService_SendEmailStudentRegister_ReturnTrue()
    {
        // Arrange
        string receiver = "receiver@gmail.com";
        var service = new EmailService();

        // Act
        var result = service.SendEmailStudentRegister(receiver);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void EmailService_SendEmailStudentRegisterApprove_ReturnFalse()
    {
        // Arrange
        string receiver = "receiver";
        var service = new EmailService();

        // Act
        var result = service.SendEmailStudentRegisterApprove(receiver);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EmailService_SendEmailStudentRegisterApprove_ReturnTrue()
    {
        // Arrange
        string receiver = "receiver@gmail.com";
        var service = new EmailService();

        // Act
        var result = service.SendEmailStudentRegisterApprove(receiver);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void EmailService_SendEmailStudentRegisterReject_ReturnFalse()
    {
        // Arrange
        string receiver = "receiver";
        var service = new EmailService();

        // Act
        var result = service.SendEmailStudentRegisterReject(receiver);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EmailService_SendEmailStudentRegisterReject_ReturnTrue()
    {
        // Arrange
        string receiver = "receiver@gmail.com";
        var service = new EmailService();

        // Act
        var result = service.SendEmailStudentRegisterReject(receiver);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void EmailService_SendEmailVerification()
    {
        // Arrange
        string receiver = "receiver@gmail.com";
        var service = new EmailService();

        // Act
        var result = service.SendEmailVerification(receiver);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(string));
    }
}
