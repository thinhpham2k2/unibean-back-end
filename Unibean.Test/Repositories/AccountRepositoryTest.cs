using Microsoft.EntityFrameworkCore;
using Unibean.Repository.Entities;

namespace Unibean.Test.Repositories;

public class AccountRepositoryTest
{
    private async Task<UnibeanDBContext> UnibeanDBContext()
    {
        var options = new DbContextOptionsBuilder<UnibeanDBContext>()
            .UseInMemoryDatabase(databaseName: Ulid.NewUlid().ToString())
            .Options;
        var databaseContext = new UnibeanDBContext();
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
                    Id = Ulid.NewUlid().ToString(),
                    Role = randomRole,
                    UserName = "username" + i,
                    Password = "password" + i,
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
}
