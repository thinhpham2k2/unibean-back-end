using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Unibean.Test.Repositories;

public class AccountRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Accounts.AnyAsync())
        {
            Array values = Enum.GetValues(typeof(Role));
            for (int i = 1; i <= 10; i++)
            {
                Random random = new();
                Role randomRole = (Role)values.GetValue(random.Next(values.Length));
                databaseContext.Accounts.Add(
                new Account()
                {
                    Id = i.ToString(),
                    Role = randomRole,
                    UserName = "username" + i,
                    Password = BCryptNet.HashPassword(i.ToString()),
                    Phone = "phone" + i,
                    Email = "email" + i,
                    Avatar = "avatar" + i,
                    FileName = "fileName" + i,
                    IsVerify = true,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    DateVerified = DateTime.Now,
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
    public async void AccountRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new AccountRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id,
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Account>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void AccountRepository_CheckEmailDuplicate_ReturnFalse()
    {
        // Arrange
        string email = "email1";
        var dbContext = await UnibeanDBContext();
        var repository = new AccountRepository(dbContext);

        // Act & Assert
        Assert.False(repository.CheckEmailDuplicate(email));
    }

    [Fact]
    public async void AccountRepository_CheckEmailDuplicate_ReturnTrue()
    {
        // Arrange
        string email = "email";
        var dbContext = await UnibeanDBContext();
        var repository = new AccountRepository(dbContext);

        // Act & Assert
        Assert.True(repository.CheckEmailDuplicate(email));
    }

    [Fact]
    public async void AccountRepository_CheckPhoneDuplicate_ReturnFalse()
    {
        // Arrange
        string phone = "phone1";
        var dbContext = await UnibeanDBContext();
        var repository = new AccountRepository(dbContext);

        // Act & Assert
        Assert.False(repository.CheckPhoneDuplicate(phone));
    }

    [Fact]
    public async void AccountRepository_CheckPhoneDuplicate_ReturnTrue()
    {
        // Arrange
        string phone = "phone";
        var dbContext = await UnibeanDBContext();
        var repository = new AccountRepository(dbContext);

        // Act & Assert
        Assert.True(repository.CheckPhoneDuplicate(phone));
    }

    [Fact]
    public async void AccountRepository_CheckUsernameDuplicate_ReturnFalse()
    {
        // Arrange
        string userName = "username1";
        var dbContext = await UnibeanDBContext();
        var repository = new AccountRepository(dbContext);

        // Act & Assert
        Assert.False(repository.CheckUsernameDuplicate(userName));
    }

    [Fact]
    public async void AccountRepository_CheckUsernameDuplicate_ReturnTrue()
    {
        // Arrange
        string userName = "username";
        var dbContext = await UnibeanDBContext();
        var repository = new AccountRepository(dbContext);

        // Act & Assert
        Assert.True(repository.CheckUsernameDuplicate(userName));
    }

    [Fact]
    public async void AccountRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new AccountRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
    }

    [Fact]
    public async void AccountRepository_GetByEmail()
    {
        // Arrange
        string id = "1";
        string email = "email" + id;
        var dbContext = await UnibeanDBContext();
        var repository = new AccountRepository(dbContext);

        // Act
        var result = repository.GetByEmail(email);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Account>();
        Assert.Equal(id, result.Id);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async void AccountRepository_GetById()
    {
        // Arrange
        string id = "1";
        string email = "email" + id;
        var dbContext = await UnibeanDBContext();
        var repository = new AccountRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Account>();
        Assert.Equal(id, result.Id);
        Assert.Equal(email, result.Email);
    }

    [Fact]
    public async void AccountRepository_GetByUserNameAndPassword()
    {
        // Arrange
        string id = "1";
        string email = "email" + id;
        string userName = "username" + id;
        string password = id;
        var dbContext = await UnibeanDBContext();
        var repository = new AccountRepository(dbContext);

        // Act
        var result = repository.GetByUserNameAndPassword(userName, password);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Account>();
        Assert.Equal(id, result.Id);
        Assert.Equal(email, result.Email);
        Assert.Equal(userName, result.UserName);
    }

    [Fact]
    public async void AccountRepository_Update()
    {
        // Arrange
        string id = "1";
        string email = "email";
        var dbContext = await UnibeanDBContext();
        var repository = new AccountRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Accounts.FindAsync(id);
        existingAccount.Email = email;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Account>();
        Assert.Equal(id, result.Id);
        Assert.Equal(email, result.Email);
    }
}
