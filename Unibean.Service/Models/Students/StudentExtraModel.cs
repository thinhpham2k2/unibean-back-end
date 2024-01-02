﻿using Unibean.Service.Models.Orders;
using Unibean.Service.Models.StudentChallenges;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Models.VoucherItems;

namespace Unibean.Service.Models.Students;

public class StudentExtraModel
{
    public string Id { get; set; }
    public string LevelId { get; set; }
    public string LevelName { get; set; }
    public string GenderId { get; set; }
    public string GenderName { get; set; }
    public string MajorId { get; set; }
    public string MajorName { get; set; }
    public string CampusId { get; set; }
    public string CampusName { get; set; }
    public string AccountId { get; set; }
    public string UserName { get; set; }
    public string StudentCardFront { get; set; }
    public string FileNameFront { get; set; }
    public string StudentCardBack { get; set; }
    public string FileNameBack { get; set; }
    public string FullName { get; set; }
    public string Code { get; set; }
    public string InviteCode { get; set; }
    public string Email { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string Phone { get; set; }
    public string Avatar { get; set; }
    public string ImageName { get; set; }
    public string Address { get; set; }
    public decimal? TotalIncome { get; set; }
    public decimal? TotalSpending { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public DateTime? DateVerified { get; set; }
    public bool? IsVerify { get; set; }
    public bool? State { get; set; }
    public bool? Status { get; set; }
    public decimal? GreenWallet { get; set; }
    public decimal? RedWallet { get; set; }
    public decimal? Following { get; set; }
    public string Inviter { get; set; }
    public decimal? Invitee { get; set; }   
    public List<VoucherItemModel> VoucherItems { get; set; }
    public List<TransactionModel> Transactions { get; set; } // All transaction
    public List<OrderModel> Orders { get; set; }
    public List<StudentChallengeModel> Challenges { get; set; }
}