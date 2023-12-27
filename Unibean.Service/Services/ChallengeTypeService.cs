using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.ChallengeTypes;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class ChallengeTypeService : IChallengeTypeService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "challengeTypes";

    private readonly IChallengeTypeRepository challengeTypeRepository;

    private readonly IFireBaseService fireBaseService;

    public ChallengeTypeService(IChallengeTypeRepository challengeTypeRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<ChallengeType, ChallengeTypeModel>().ReverseMap();
            cfg.CreateMap<PagedResultModel<ChallengeType>, PagedResultModel<ChallengeTypeModel>>()
            .ReverseMap();
            cfg.CreateMap<ChallengeType, UpdateChallengeTypeModel>()
            .ReverseMap()
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<ChallengeType, CreateChallengeTypeModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.challengeTypeRepository = challengeTypeRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<ChallengeTypeModel> Add(CreateChallengeTypeModel creation)
    {
        ChallengeType entity = mapper.Map<ChallengeType>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<ChallengeTypeModel>(challengeTypeRepository.Add(entity));
    }

    public void Delete(string id)
    {
        ChallengeType entity = challengeTypeRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Image != null && entity.FileName != null)
            {
                //Remove image
                fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
            }
            challengeTypeRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Not found challenge's type");
        }
    }

    public PagedResultModel<ChallengeTypeModel> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<ChallengeTypeModel>>(challengeTypeRepository.GetAll(propertySort, isAsc, search, page, limit));
    }

    public ChallengeTypeModel GetById(string id)
    {
        ChallengeType entity = challengeTypeRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<ChallengeTypeModel>(entity);
        }
        throw new InvalidParameterException("Not found challenge's type");
    }

    public async Task<ChallengeTypeModel> Update(string id, UpdateChallengeTypeModel update)
    {
        ChallengeType entity = challengeTypeRepository.GetById(id);
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
            return mapper.Map<ChallengeTypeModel>(challengeTypeRepository.Update(entity));
        }
        throw new InvalidParameterException("Not found challenge's type");
    }
}
