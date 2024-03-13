using FakeItEasy;
using Unibean.Repository.Repositories.Interfaces;

namespace Unibean.Test.Services;

public class ChartServiceTest
{
    private readonly IAdminRepository adminRepository;

    private readonly IBrandRepository brandRepository;

    private readonly ICampaignRepository campaignRepository;

    private readonly IProductRepository productRepository;

    private readonly IStaffRepository staffRepository;

    private readonly IStoreRepository storeRepository;

    private readonly IOrderRepository orderRepository;

    private readonly IStudentRepository studentRepository;

    private readonly IActivityRepository activityRepository;

    private readonly IVoucherItemRepository voucherItemRepository;

    private readonly IOrderTransactionRepository orderTransactionRepository;

    private readonly IActivityTransactionRepository activityTransactionRepository;

    private readonly IRequestTransactionRepository requestTransactionRepository;

    private readonly ICampaignTransactionRepository campaignTransactionRepository;

    private readonly IBonusTransactionRepository bonusTransactionRepository;

    public ChartServiceTest()
    {
        adminRepository = A.Fake<IAdminRepository>();
        brandRepository = A.Fake<IBrandRepository>();
        campaignRepository = A.Fake<ICampaignRepository>();
        productRepository = A.Fake<IProductRepository>();
        staffRepository = A.Fake<IStaffRepository>();
        storeRepository = A.Fake<IStoreRepository>();
        orderRepository = A.Fake<IOrderRepository>();
        studentRepository = A.Fake<IStudentRepository>();
        activityRepository = A.Fake<IActivityRepository>();
        voucherItemRepository = A.Fake<IVoucherItemRepository>();
        orderTransactionRepository = A.Fake<IOrderTransactionRepository>();
        activityTransactionRepository = A.Fake<IActivityTransactionRepository>();
        requestTransactionRepository = A.Fake<IRequestTransactionRepository>();
        campaignTransactionRepository = A.Fake<ICampaignTransactionRepository>();
        bonusTransactionRepository = A.Fake<IBonusTransactionRepository>();
    }
}
