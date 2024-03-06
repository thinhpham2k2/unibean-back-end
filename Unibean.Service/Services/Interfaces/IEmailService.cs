using Unibean.Repository.Entities;

namespace Unibean.Service.Services.Interfaces;

public interface IEmailService
{
    bool SendEmailBrandRegister(string receiver);

    bool SendEmailCamapaignClose(List<string> receivers, string campaignName);

    bool SendEmailStudentRegister(string receiver);

    bool SendEmailStudentRegisterApprove(string receiver);

    bool SendEmailStudentRegisterReject(string receiver);

    string SendEmailVerification(string receiver);

    bool SendEmailAbortOrder(string receiver, string orderId);

    bool SendEmailCreateOrder(string receiver, string studentCode, Order order);
}
