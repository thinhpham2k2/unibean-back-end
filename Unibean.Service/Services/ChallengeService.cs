using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Challenges;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class ChallengeService : IChallengeService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "challenges";

    private readonly IChallengeRepository challengeRepository;

    private readonly IFireBaseService fireBaseService;

    public ChallengeService(IChallengeRepository challengeRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Challenge, ChallengeModel>()
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => src.Type.TypeName))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Challenge>, PagedResultModel<ChallengeModel>>()
            .ReverseMap();
            cfg.CreateMap<Challenge, UpdateChallengeModel>()
            .ReverseMap()
            .ForMember(t => t.Type, opt => opt.MapFrom(src => (string)null))
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<Challenge, CreateChallengeModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.challengeRepository = challengeRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<ChallengeModel> Add(CreateChallengeModel creation)
    {
        Challenge entity = mapper.Map<Challenge>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<ChallengeModel>(challengeRepository.Add(entity));
    }

    public void Delete(string id)
    {
        Challenge entity = challengeRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Image != null && entity.FileName != null)
            {
                //Remove image
                fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
            }
            challengeRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Not found challenge");
        }
    }

    public PagedResultModel<ChallengeModel> GetAll
        (List<string> typeIds, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<ChallengeModel>>(challengeRepository.GetAll
            (typeIds, propertySort, isAsc, search, page, limit));
    }

    public ChallengeModel GetById(string id)
    {
        Challenge entity = challengeRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<ChallengeModel>(entity);
        }
        throw new InvalidParameterException("Not found challenge");
    }

    public async Task<ChallengeModel> Update(string id, UpdateChallengeModel update)
    {
        Challenge entity = challengeRepository.GetById(id);
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
            return mapper.Map<ChallengeModel>(challengeRepository.Update(entity));
        }
        throw new InvalidParameterException("Not found challenge");
    }
}
