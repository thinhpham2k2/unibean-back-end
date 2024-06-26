﻿using AutoMapper;
using Enable.EnumDisplayName;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Transactions;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class ActivityTransactionService : IActivityTransactionService
{
    private readonly Mapper mapper;

    private readonly IActivityTransactionRepository activityTransactionRepo;

    public ActivityTransactionService(
        IActivityTransactionRepository activityTransactionRepo)
    {
        var config = new MapperConfiguration(cfg
            =>
        {
            cfg.CreateMap<ActivityTransaction, TransactionModel>()
            .ForMember(t => t.Name, opt => opt.MapFrom(src => src.Activity.VoucherItem.Voucher.VoucherName))
            .ForMember(t => t.RequestId, opt => opt.MapFrom(src => src.ActivityId))
            .ForMember(t => t.WalletTypeId, opt => opt.MapFrom(src => (int)src.Wallet.Type))
            .ForMember(t => t.WalletType, opt => opt.MapFrom(src => src.Wallet.Type))
            .ForMember(t => t.WalletTypeName, opt => opt.MapFrom(src => src.Wallet.Type.GetDisplayName()))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => src.Activity.Type.GetEnumDescription()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => src.Activity.DateCreated))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.activityTransactionRepo = activityTransactionRepo;
    }

    public List<TransactionModel> GetAll
        (List<string> walletIds, List<string> activityIds, string search)
    {
        return mapper.Map<List<TransactionModel>>(activityTransactionRepo.GetAll
            (walletIds, activityIds, search));
    }
}
