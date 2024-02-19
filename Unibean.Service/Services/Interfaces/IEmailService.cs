namespace Unibean.Service.Services.Interfaces;

public interface IEmailService
{
    bool SendEmailBrandRegister(string receiver);

    bool SendEmailStudentRegister(string receiver);

    bool SendEmailStudentRegisterApprove(string receiver);

    bool SendEmailStudentRegisterReject(string receiver);

    string SendEmailVerification(string receiver);
}
