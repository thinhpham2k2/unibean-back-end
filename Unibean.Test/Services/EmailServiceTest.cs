using FluentAssertions;
using Unibean.Repository.Entities;
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

    [Fact]
    public void EmailService_SendEmailAbortOrder_ReturnFalse()
    {
        // Arrange
        string receiver = "receiver";
        string orderId = "01HQJE16SX45YXF9SM2GNAENS9";
        var service = new EmailService();

        // Act
        var result = service.SendEmailAbortOrder(receiver, orderId);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EmailService_SendEmailAbortOrder_ReturnTrue()
    {
        // Arrange
        string receiver = "receiver@gmail.com";
        string orderId = "01HQJE16SX45YXF9SM2GNAENS9";
        var service = new EmailService();

        // Act
        var result = service.SendEmailAbortOrder(receiver, orderId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void EmailService_SendEmailCreateOrder_ReturnFalse()
    {
        // Arrange
        string receiver = "receiver";
        string studentCode = "123";
        string studentName = "Phạm Quốc Thịnh";
        Order order = new()
        {
            Id = "01HQJE16SX45YXF9SM2GNAENS9",
            Amount = 120000,
            Station = new()
            {
                StationName = "Sảnh gương (Đối diện phòng 117)",
                Address = "Địa chỉ: Lô E2a-7, Đường D1 Khu Công nghệ cao, P. Long Thạnh Mỹ, TP. Thủ Đức, TP. Hồ Chí Minh"
            },
            OrderDetails = new List<OrderDetail>()
            {
                new()
                {
                    Product = new()
                    {
                        ProductName = "Unibean Logo",
                        Price = 30000,
                        Images = new List<Image>()
                        {
                            new()
                            {
                                IsCover = true,
                                Url = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/assets%2FlogoUB.png?alt=media&token=cc25afda-8384-43c8-b0de-a69cab2c86c2"
                            }
                        }
                    },
                    Quantity = 4,
                    Amount = 120000
                }
            }
        };
        var service = new EmailService();

        // Act
        var result = service.SendEmailCreateOrder(receiver, studentCode, studentName, order);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EmailService_SendEmailCreateOrder_ReturnTrue()
    {
        // Arrange
        string receiver = "receiver@gmail.com";
        string studentCode = "123";
        string studentName = "Phạm Quốc Thịnh";
        Order order = new()
        {
            Id = "01HQJE16SX45YXF9SM2GNAENS9",
            Amount = 120000,
            Station = new()
            {
                StationName = "Sảnh gương (Đối diện phòng 117)",
                Address = "Địa chỉ: Lô E2a-7, Đường D1 Khu Công nghệ cao, P. Long Thạnh Mỹ, TP. Thủ Đức, TP. Hồ Chí Minh"
            },
            OrderDetails = new List<OrderDetail>()
            {
                new()
                {
                    Product = new()
                    {
                        ProductName = "Unibean Logo",
                        Price = 30000,
                        Images = new List<Image>()
                        {
                            new()
                            {
                                IsCover = true,
                                Url = "https://firebasestorage.googleapis.com/v0/b/upload-file-2ac29.appspot.com/o/assets%2FlogoUB.png?alt=media&token=cc25afda-8384-43c8-b0de-a69cab2c86c2"
                            }
                        }
                    },
                    Quantity = 4,
                    Amount = 120000
                }
            }
        };
        var service = new EmailService();

        // Act
        var result = service.SendEmailCreateOrder(receiver, studentCode, studentName, order);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void EmailService_SendEmailCamapaign_ReturnFalse()
    {
        // Arrange
        CampaignState state = CampaignState.Pending;
        string receiver = "receiver";
        string brandName = "brandName";
        string campaignName = "campaignName";
        string note = "note";
        var service = new EmailService();

        // Act
        var result = service.SendEmailCamapaign(state, receiver, brandName, campaignName, note);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void EmailService_SendEmailCamapaign_ReturnTrue()
    {
        // Arrange
        CampaignState state = CampaignState.Pending;
        string receiver = "receiver@gmail.com";
        string brandName = "brandName";
        string campaignName = "campaignName";
        string note = "note";
        var service = new EmailService();

        // Act
        var result = service.SendEmailCamapaign(state, receiver, brandName, campaignName, note);

        // Assert
        Assert.True(result);
    }
}
