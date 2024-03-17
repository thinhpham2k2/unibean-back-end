using FakeItEasy;
using FluentAssertions;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services;

namespace Unibean.Test.Services;

public class TransactionServiceTest
{
    private readonly ITransactionRepository transactionRepository;

    public TransactionServiceTest()
    {
        transactionRepository = A.Fake<ITransactionRepository>();
    }

    [Fact]
    public void TransactionService_GetAll()
    {
        // Arrange
        List<string> walletIds = new();
        List<TransactionType> typeIds = new();
        bool? state = null;
        string propertySort = "Id";
        bool isAsc = true;
        string search = "";
        int page = 1;
        int limit = 10;
        Role role = Role.Brand;
        A.CallTo(() => transactionRepository.GetAll(walletIds, typeIds, search, role));
        var service = new TransactionService(transactionRepository);

        // Act
        var result = service.GetAll(walletIds, typeIds, state, propertySort, isAsc, 
            search, page, limit, role);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(PagedResultModel<TransactionModel>));
    }
}
