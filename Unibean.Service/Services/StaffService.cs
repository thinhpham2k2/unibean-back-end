using AutoMapper;
using Enable.EnumDisplayName;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Staffs;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Unibean.Service.Services;

public class StaffService : IStaffService
{
    private readonly Mapper mapper;

    private readonly string ACCOUNT_FOLDER_NAME = "accounts";

    private readonly IStaffRepository staffRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IAccountRepository accountRepository;

    public StaffService(
        IStaffRepository staffRepository,
        IFireBaseService fireBaseService,
        IAccountRepository accountRepository)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Staff, StaffModel>()
            .ForMember(a => a.StationName, opt => opt.MapFrom(src => src.Station.StationName))
            .ForMember(a => a.UserName, opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(a => a.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ForMember(a => a.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(a => a.Avatar, opt => opt.MapFrom(src => src.Account.Avatar))
            .ForMember(a => a.FileName, opt => opt.MapFrom(src => src.Account.FileName))
            .ForMember(a => a.Description, opt => opt.MapFrom(src => src.Account.Description))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Staff>, PagedResultModel<StaffModel>>()
            .ReverseMap();
            cfg.CreateMap<Staff, StaffExtraModel>()
            .ForMember(a => a.StationName, opt => opt.MapFrom(src => src.Station.StationName))
            .ForMember(a => a.StationImage, opt => opt.MapFrom(src => src.Station.Image))
            .ForMember(a => a.StationStateId, opt => opt.MapFrom(src => src.Station.State))
            .ForMember(a => a.StationState, opt => opt.MapFrom(src => src.Station.State))
            .ForMember(a => a.StationStateName, opt => opt.MapFrom(src => src.Station.State.GetDisplayName()))
            .ForMember(a => a.UserName, opt => opt.MapFrom(src => src.Account.UserName))
            .ForMember(a => a.Phone, opt => opt.MapFrom(src => src.Account.Phone))
            .ForMember(a => a.Email, opt => opt.MapFrom(src => src.Account.Email))
            .ForMember(a => a.Avatar, opt => opt.MapFrom(src => src.Account.Avatar))
            .ForMember(a => a.FileName, opt => opt.MapFrom(src => src.Account.FileName))
            .ForMember(a => a.Description, opt => opt.MapFrom(src => src.Account.Description))
            .ReverseMap();
            // Map Create Staff Model
            cfg.CreateMap<Staff, CreateStaffModel>()
            .ReverseMap()
            .ForMember(a => a.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(p => p.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.Status, opt => opt.MapFrom(src => true));
            cfg.CreateMap<Account, CreateStaffModel>()
            .ReverseMap()
            .ForMember(a => a.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(a => a.Role, opt => opt.MapFrom(src => Role.Staff))
            .ForMember(a => a.Password, opt => opt.MapFrom(src => BCryptNet.HashPassword(src.Password)))
            .ForMember(a => a.IsVerify, opt => opt.MapFrom(src => true))
            .ForMember(a => a.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(a => a.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(a => a.DateVerified, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(a => a.Status, opt => opt.MapFrom(src => true));
            // Map Update Staff Model
            cfg.CreateMap<Staff, UpdateStaffModel>()
            .ReverseMap()
            .ForMember(a => a.Station, opt => opt.MapFrom(src => (string)null))
            .ForMember(a => a.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForPath(a => a.Account.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForPath(a => a.Account.Description, opt => opt.MapFrom(src => src.Description))
            .ForPath(a => a.Account.State, opt => opt.MapFrom(src => src.State));
        });
        mapper = new Mapper(config);
        this.staffRepository = staffRepository;
        this.fireBaseService = fireBaseService;
        this.accountRepository = accountRepository;
    }

    public async Task<StaffExtraModel> Add(CreateStaffModel creation)
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
        Staff staff = mapper.Map<Staff>(creation);
        staff.AccountId = account.Id;

        return mapper.Map<StaffExtraModel>(staffRepository.Add(staff));
    }

    public void Delete(string id)
    {
        Staff entity = staffRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Station == null || entity.Station.Staffs.Count > 1)
            {
                // Avatar
                if (entity.Account.Avatar != null && entity.Account.Avatar.Length > 0)
                {
                    // Remove image
                    fireBaseService.RemoveFileAsync(entity.Account.FileName, ACCOUNT_FOLDER_NAME);
                }

                staffRepository.Delete(id);
                accountRepository.Delete(entity.Account.Id);
            }
            else
            {
                throw new InvalidParameterException("Xóa thất bại do cần tồn tại ít nhất 1 nhân viên thuộc trạm");
            }
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy nhân viên");
        }
    }

    public PagedResultModel<StaffModel> GetAll
        (List<string> stationIds, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<StaffModel>>
            (staffRepository.GetAll(stationIds, state, propertySort, isAsc, search, page, limit));
    }

    public StaffExtraModel GetById(string id)
    {
        Staff entity = staffRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<StaffExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy nhân viên");
    }

    public async Task<StaffExtraModel> Update(string id, UpdateStaffModel update)
    {
        Staff entity = staffRepository.GetById(id);
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

            return mapper.Map<StaffExtraModel>(staffRepository.Update(entity));
        }
        throw new InvalidParameterException("Không tìm thấy nhân viên");
    }
}
