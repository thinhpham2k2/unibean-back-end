namespace Unibean.Service.Services.Interfaces;

public interface IEmailService
{
    string SendEmailVerification(string receiver);
}
