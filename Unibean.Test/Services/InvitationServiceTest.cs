using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Admins;
using Unibean.Service.Models.Invitations;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class InvitationServiceTest
{
    private readonly IInvitationRepository invitationRepository;

    public InvitationServiceTest()
    {
        invitationRepository = A.Fake<IInvitationRepository>();
    }

    [Fact]
    public void InvitationService_Add()
    {
        // Arrange
        CreateInvitationModel creation = A.Fake<CreateInvitationModel>();
        A.CallTo(() => invitationRepository.Add(A<Invitation>.Ignored)).Returns(new());
        var service = new InvitationService(invitationRepository);

        // Act
        var result = service.Add(creation);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(InvitationModel));
    }

    [Fact]
    public void InvitationService_ExistInvitation()
    {
        // Arrange
        string invitee = "invitee";
        A.CallTo(() => invitationRepository.ExistInvitation(invitee)).Returns(true);
        var service = new InvitationService(invitationRepository);

        // Act
        var result = service.ExistInvitation(invitee);

        // Assert
        Assert.True(result);
    }
}
