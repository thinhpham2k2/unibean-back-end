﻿using Unibean.Repository.Entities;

namespace Unibean.Service.Services.Interfaces;

public interface IEmailService
{
    bool SendEmailBrandRegister(string receiver);

    bool SendEmailCamapaignClose(List<string> receivers, string campaignName);

    bool SendEmailStudentRegister(string receiver);

    bool SendEmailStudentRegisterApprove(string receiver);

    bool SendEmailStudentRegisterReject(string receiver, string note);

    string SendEmailVerification(string receiver);

    bool SendEmailAbortOrder(string receiver, string orderId, string note);

    bool SendEmailCreateOrder(string receiver, string studentCode, string studentName, Order order);

    bool SendEmailCamapaign(CampaignState state, string receiver,
        string brandName, string campaignName, string note);
}
