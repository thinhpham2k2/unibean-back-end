using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Admins;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class AdminService : IAdminService
{
    private readonly Mapper mapper;

    private readonly IAdminRepository adminRepository;

    private readonly IFireBaseService fireBaseService;

    public AdminService(IAdminRepository adminRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Admin, AdminModel>().ReverseMap();
        });
        mapper = new Mapper(config);
        this.adminRepository = adminRepository;
        this.fireBaseService = fireBaseService;
    }
}
