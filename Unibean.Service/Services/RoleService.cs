using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Roles;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class RoleService : IRoleService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "roles";

    private readonly IRoleRepository roleRepository;

    private readonly IFireBaseService fireBaseService;

    public RoleService(IRoleRepository roleRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Role, RoleModel>().ReverseMap();
            cfg.CreateMap<PagedResultModel<Role>, PagedResultModel<RoleModel>>()
            .ReverseMap();
            cfg.CreateMap<Role, UpdateRoleModel>()
            .ReverseMap()
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<Role, CreateRoleModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.roleRepository = roleRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<RoleModel> Add(CreateRoleModel creation)
    {
        Role entity = mapper.Map<Role>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<RoleModel>(roleRepository.Add(entity));
    }

    public void Delete(string id)
    {
        Role entity = roleRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Image != null && entity.FileName != null)
            {
                //Remove image
                fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
            }
            roleRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Not found role");
        }
    }

    public PagedResultModel<RoleModel> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<RoleModel>>(roleRepository.GetAll(propertySort, isAsc, search, page, limit));
    }

    public RoleModel GetById(string id)
    {
        Role entity = roleRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<RoleModel>(entity);
        }
        throw new InvalidParameterException("Not found role");
    }

    public RoleModel GetRoleByName(string roleName)
    {
        Role entity = roleRepository.GetRoleByName(roleName);
        if (entity != null)
        {
            return mapper.Map<RoleModel>(entity);
        }
        throw new InvalidParameterException("Not found role");
    }

    public async Task<RoleModel> Update(string id, UpdateRoleModel update)
    {
        Role entity = roleRepository.GetById(id);
        if (entity != null)
        {
            entity = mapper.Map(update, entity);
            if (update.Image != null && update.Image.Length > 0)
            {
                // Remove image
                await fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);

                //Upload new image update
                FireBaseFile f = await fireBaseService.UploadFileAsync(update.Image, FOLDER_NAME);
                entity.Image = f.URL;
                entity.FileName = f.FileName;
            }
            return mapper.Map<RoleModel>(roleRepository.Update(entity));
        }
        throw new InvalidParameterException("Not found role");
    }
}
