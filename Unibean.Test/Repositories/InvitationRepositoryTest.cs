using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class InvitationRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Invitations.AnyAsync())
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Invitations.Add(
                new Invitation()
                {
                    Id = i.ToString(),
                    InviteeId = i.ToString(),
                    InviterId = i.ToString(),
                    DateCreated = DateTime.Now,
                    Description = "description" + i,
                    State = true,
                    Status = true,
                });
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }

    [Fact]
    public async void InvitationRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new InvitationRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Invitation>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void InvitationRepository_ExistInvitation_ReturnFalse()
    {
        // Arrange
        string invitee = "11";
        var dbContext = await UnibeanDBContext();
        var repository = new InvitationRepository(dbContext);

        // Act & Assert
        Assert.False(repository.ExistInvitation(invitee));
    }

    [Fact]
    public async void InvitationRepository_ExistInvitation_ReturnTrue()
    {
        // Arrange
        string invitee = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new InvitationRepository(dbContext);

        // Act & Assert
        Assert.True(repository.ExistInvitation(invitee));
    }
}
