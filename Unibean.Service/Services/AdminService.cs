﻿using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Admins;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Unibean.Service.Services;

public class AdminService : IAdminService
{
    private readonly Mapper mapper;

    private readonly string ACCOUNT_FOLDER_NAME = "accounts";

    private readonly IAdminRepository adminRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IAccountRepository accountRepository;

    public AdminService(IAdminRepository adminRepository,
        IFireBaseService fireBaseService,
        IAccountRepository accountRepository)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Admin, AdminModel>()
            .ForMember(a => a.UserName, opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(a => a.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ForMember(a => a.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(a => a.Avatar, opt => opt.MapFrom(src => src.Account.Avatar))
            .ForMember(a => a.FileName, opt => opt.MapFrom(src => src.Account.FileName))
            .ForMember(a => a.Description, opt => opt.MapFrom(src => src.Account.Description))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Admin>, PagedResultModel<AdminModel>>()
            .ReverseMap();
            cfg.CreateMap<Admin, AdminExtraModel>()
            .ForMember(a => a.UserName, opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(a => a.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ForMember(a => a.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(a => a.Avatar, opt => opt.MapFrom(src => src.Account.Avatar))
            .ForMember(a => a.FileName, opt => opt.MapFrom(src => src.Account.FileName))
            .ForMember(a => a.Description, opt => opt.MapFrom(src => src.Account.Description))
            .ForMember(a => a.NumberOfRequests, opt => opt.MapFrom(src => src.Requests.Count))
            .ForMember(a => a.AmountOfRequests, opt => opt.MapFrom(
                src => src.Requests.SelectMany(r => r.RequestTransactions.Select(t => t.Amount)).Sum()))
            .ReverseMap();
            // Map Create Admin Model
            cfg.CreateMap<Admin, CreateAdminModel>()
            .ReverseMap()
            .ForMember(a => a.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(p => p.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.Status, opt => opt.MapFrom(src => true));
            cfg.CreateMap<Account, CreateAdminModel>()
            .ReverseMap()
            .ForMember(a => a.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(a => a.Role, opt => opt.MapFrom(src => Role.Admin))
            .ForMember(a => a.Password, opt => opt.MapFrom(src => BCryptNet.HashPassword(src.Password)))
            .ForMember(a => a.IsVerify, opt => opt.MapFrom(src => true))
            .ForMember(a => a.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(a => a.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(a => a.DateVerified, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(a => a.Status, opt => opt.MapFrom(src => true));
            // Map Update Admin Model
            cfg.CreateMap<Admin, UpdateAdminModel>()
            .ReverseMap()
            .ForMember(a => a.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForPath(a => a.Account.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForPath(a => a.Account.Description, opt => opt.MapFrom(src => src.Description))
            .ForPath(a => a.Account.State, opt => opt.MapFrom(src => src.State));
        });
        mapper = new Mapper(config);
        this.adminRepository = adminRepository;
        this.fireBaseService = fireBaseService;
        this.accountRepository = accountRepository;
    }

    public async Task<AdminExtraModel> Add(CreateAdminModel creation)
    {
        Account account = mapper.Map<Account>(creation);

        //Upload avatar
        if (creation.Avatar != null && creation.Avatar.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Avatar, ACCOUNT_FOLDER_NAME);
            account.Avatar = f.URL;
            account.FileName = f.FileName;
        }

        account = accountRepository.Add(account);
        Admin admin = mapper.Map<Admin>(creation);
        admin.AccountId = account.Id;

        return mapper.Map<AdminExtraModel>(adminRepository.Add(admin));
    }

    public void Delete(string id)
    {
        Admin entity = adminRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Requests.Count.Equals(0))
            {
                // Avatar
                if (entity.Account.Avatar != null && entity.Account.Avatar.Length > 0)
                {
                    // Remove image
                    fireBaseService.RemoveFileAsync(entity.Account.FileName, ACCOUNT_FOLDER_NAME);
                }

                adminRepository.Delete(id);
                accountRepository.Delete(entity.Account.Id);
            }
            else
            {
                throw new InvalidParameterException("Không thể xóa quản trị viên do tồn tại yêu cầu nạp bởi tài khoản này");
            }
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy quản trị viên");
        }
    }

    public PagedResultModel<AdminModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<AdminModel>>
            (adminRepository.GetAll(state, propertySort, isAsc, search, page, limit));
    }

    public AdminExtraModel GetById(string id)
    {
        Admin entity = adminRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<AdminExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy quản trị viên");
    }

    public async Task<AdminExtraModel> Update(string id, UpdateAdminModel update)
    {
        Admin entity = adminRepository.GetById(id);
        if (entity != null)
        {
            entity = mapper.Map(update, entity);

            // Avatar
            if (update.Avatar != null && update.Avatar.Length > 0)
            {
                // Remove image
                await fireBaseService.RemoveFileAsync(entity.Account.FileName, ACCOUNT_FOLDER_NAME);

                //Upload new image update
                FireBaseFile f = await fireBaseService.UploadFileAsync(update.Avatar, ACCOUNT_FOLDER_NAME);
                entity.Account.Avatar = f.URL;
                entity.Account.FileName = f.FileName;
            }

            return mapper.Map<AdminExtraModel>(adminRepository.Update(entity));
        }
        throw new InvalidParameterException("Không tìm thấy quản trị viên");
    }
}
