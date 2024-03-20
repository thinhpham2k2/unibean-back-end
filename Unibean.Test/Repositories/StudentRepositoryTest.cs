using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories;

namespace Unibean.Test.Repositories;

public class StudentRepositoryTest
{
    private static async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext(options);
        databaseContext.Database.EnsureCreated();
        if (!await databaseContext.Students.AnyAsync())
        {
            Array values = Enum.GetValues(typeof(StudentState));
            Array values1 = Enum.GetValues(typeof(Gender));
            for (int i = 1; i <= 10; i++)
            {
                Random random = new();
                StudentState randomState =
                    (StudentState)values.GetValue(random.Next(values.Length));
                Gender randomGender =
                    (Gender)values.GetValue(random.Next(values1.Length));
                databaseContext.Students.Add(
                new Student()
                {
                    Id = i.ToString(),
                    MajorId = i.ToString(),
                    AccountId = i.ToString(),
                    CampusId = i.ToString(),
                    StudentCardFront = "studentCardFront" + i,
                    FileNameFront = "fileNameFront" + i,
                    StudentCardBack = "studentCardBack" + i,
                    FileNameBack = "fileNameBack" + i,
                    FullName = "fullName" + i,
                    Code = "code" + i,
                    Address = "address" + i,
                    TotalIncome = 0,
                    TotalSpending = 0,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    State = randomState,
                    Gender = randomGender,
                    Status = true,
                    Wallets = new List<Wallet>()
                    {
                        new()
                        {
                            Id = i.ToString(),
                            Type = WalletType.Green,
                            Balance = 100,
                            Status = true,
                        },
                        new()
                        {
                            Id = Ulid.NewUlid().ToString(),
                            Type = WalletType.Red,
                            Balance = 100,
                            Status = true,
                        }
                    }
                });
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }

    [Fact]
    public async void StudentRepository_Add()
    {
        // Arrange
        string id = Ulid.NewUlid().ToString();
        var dbContext = await UnibeanDBContext();
        var repository = new StudentRepository(dbContext);

        // Act
        var result = repository.Add(new()
        {
            Id = id
        });

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Student>();
        Assert.Equal(id, result.Id);
    }

    [Fact]
    public async void StudentRepository_CheckCodeDuplicate_ReturnFalse()
    {
        // Arrange
        string code = "code1";
        var dbContext = await UnibeanDBContext();
        var repository = new StudentRepository(dbContext);

        // Act & Assert
        Assert.False(repository.CheckCodeDuplicate(code));
    }

    [Fact]
    public async void StudentRepository_CheckCodeDuplicate_ReturnTrue()
    {
        // Arrange
        string code = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new StudentRepository(dbContext);

        // Act & Assert
        Assert.True(repository.CheckCodeDuplicate(code));
    }

    [Fact]
    public async void StudentRepository_CheckInviteCode()
    {
        // Arrange
        string inviteCode = "11";
        var dbContext = await UnibeanDBContext();
        var repository = new StudentRepository(dbContext);

        // Act & Assert
        Assert.False(repository.CheckInviteCode(inviteCode));
    }

    [Fact]
    public async void StudentRepository_CheckStudentId_ReturnFalse()
    {
        // Arrange
        string id = "11";
        var dbContext = await UnibeanDBContext();
        var repository = new StudentRepository(dbContext);

        // Act & Assert
        Assert.False(repository.CheckStudentId(id));
    }

    [Fact]
    public async void StudentRepository_CheckStudentId_ReturnTrue()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new StudentRepository(dbContext);

        // Act & Assert
        Assert.True(repository.CheckStudentId(id));
    }

    [Fact]
    public async void StudentRepository_CountStudent()
    {
        // Arrange
        var dbContext = await UnibeanDBContext();
        var repository = new StudentRepository(dbContext);

        // Act
        var result = repository.CountStudent();

        // Assert
        result.Should().Be(10);
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async void StudentRepository_CountStudentToday()
    {
        // Arrange
        DateOnly date = DateOnly.FromDateTime(DateTime.Now);
        var dbContext = await UnibeanDBContext();
        var repository = new StudentRepository(dbContext);

        // Act
        var result = repository.CountStudentToday(date);

        // Assert
        result.Should().Be(10);
        result.Should().BeGreaterThan(0);
    }

    [Fact]
    public async void StudentRepository_Delete()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new StudentRepository(dbContext);

        // Act & Assert
        repository.Delete(id);
        Assert.False((await dbContext.Students.FindAsync(id)).Status.Value);
    }

    [Fact]
    public async void StudentRepository_GetAll()
    {
        // Arrange
        List<string> majorIds = new();
        List<string> campusIds = new();
        List<StudentState> stateIds = new();
        bool? isVerify = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        var dbContext = await UnibeanDBContext();
        var repository = new StudentRepository(dbContext);

        // Act
        var result = repository.GetAll(majorIds, campusIds, stateIds,
            isVerify, propertySort, isAsc, search, page, limit);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<PagedResultModel<Student>>();
        Assert.Equal(10, result.RowCount);
    }

    [Fact]
    public async void StudentRepository_GetById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new StudentRepository(dbContext);

        // Act
        var result = repository.GetById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Student>();
        Assert.Equal(id, result.Id);
        Assert.Equal(id, result.AccountId);
    }

    [Fact]
    public async void StudentRepository_GetByIdForValidation()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new StudentRepository(dbContext);

        // Act
        var result = repository.GetByIdForValidation(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Student>();
        Assert.Equal(id, result.Id);
        Assert.Equal(id, result.AccountId);
    }

    [Fact]
    public async void StudentRepository_GetWalletListById()
    {
        // Arrange
        string id = "1";
        var dbContext = await UnibeanDBContext();
        var repository = new StudentRepository(dbContext);

        // Act
        var result = repository.GetWalletListById(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<List<string>>();
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async void StudentRepository_Update()
    {
        // Arrange
        string id = "1";
        string fullName = "fullName";
        var dbContext = await UnibeanDBContext();
        var repository = new StudentRepository(dbContext);

        // Act
        var existingAccount = await dbContext.Students.FindAsync(id);
        existingAccount.FullName = fullName;
        var result = repository.Update(existingAccount);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Student>();
        Assert.Equal(id, result.Id);
        Assert.Equal(fullName, result.FullName);
    }
}
