using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Levels;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class LevelService : ILevelService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "levels";

    private readonly ILevelRepository levelRepository;

    private readonly IFireBaseService fireBaseService;

    public LevelService(ILevelRepository levelRepository,
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Level, LevelModel>().ReverseMap();
            cfg.CreateMap<PagedResultModel<Level>, PagedResultModel<LevelModel>>()
            .ReverseMap();
            cfg.CreateMap<Level, UpdateLevelModel>()
            .ReverseMap()
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<Level, CreateLevelModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.levelRepository = levelRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<LevelModel> Add(CreateLevelModel creation)
    {
        Level entity = mapper.Map<Level>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<LevelModel>(levelRepository.Add(entity));
    }

    public void Delete(string id)
    {
        Level entity = levelRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Image != null && entity.FileName != null)
            {
                //Remove image
                fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
            }
            levelRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Not found level");
        }
    }

    public PagedResultModel<LevelModel> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<LevelModel>>(levelRepository.GetAll(propertySort, isAsc, search, page, limit));
    }

    public LevelModel GetById(string id)
    {
        Level entity = levelRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<LevelModel>(entity);
        }
        throw new InvalidParameterException("Not found level");
    }

    public async Task<LevelModel> Update(string id, UpdateLevelModel update)
    {
        Level entity = levelRepository.GetById(id);
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
            return mapper.Map<LevelModel>(levelRepository.Update(entity));
        }
        throw new InvalidParameterException("Not found level");
    }
}
