using AutoMapper;
using Enable.EnumDisplayName;
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
            .ForMember(t => t.TypeId, opt => opt.MapFrom(src => (int)src.Type))
            .ForMember(t => t.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => src.Type.GetDisplayName()))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Challenge>, PagedResultModel<ChallengeModel>>()
            .ReverseMap();
            cfg.CreateMap<Challenge, ChallengeExtraModel>()
            .ForMember(t => t.TypeId, opt => opt.MapFrom(src => (int)src.Type))
            .ForMember(t => t.Type, opt => opt.MapFrom(src => src.Type))
            .ForMember(t => t.TypeName, opt => opt.MapFrom(src => src.Type.GetDisplayName()))
            .ForMember(t => t.TypeDescription, opt => opt.MapFrom(src => src.Type.GetEnumDescription()))
            .ForMember(t => t.NumberOfChallenges, opt => opt.MapFrom(src => src.StudentChallenges.Count))
            .ReverseMap();
            cfg.CreateMap<Challenge, UpdateChallengeModel>()
            .ReverseMap()
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

    public async Task<ChallengeExtraModel> Add(CreateChallengeModel creation)
    {
        Challenge entity = mapper.Map<Challenge>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<ChallengeExtraModel>(challengeRepository.Add(entity));
    }

    public void Delete(string id)
    {
        Challenge entity = challengeRepository.GetById(id);
        if (entity != null)
        {
            if (entity.StudentChallenges.Count.Equals(0))
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
                throw new InvalidParameterException("Xóa thất bại do tồn tại sinh viên đang thực hiện thử thách");
            }
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy thử thách");
        }
    }

    public PagedResultModel<ChallengeModel> GetAll
        (List<ChallengeType> typeIds, bool? state, string propertySort,
        bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<ChallengeModel>>(challengeRepository.GetAll
            (typeIds, state, propertySort, isAsc, search, page, limit));
    }

    public ChallengeExtraModel GetById(string id)
    {
        Challenge entity = challengeRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<ChallengeExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy thử thách");
    }

    public async Task<ChallengeExtraModel> Update(string id, UpdateChallengeModel update)
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
            return mapper.Map<ChallengeExtraModel>(challengeRepository.Update(entity));
        }
        throw new InvalidParameterException("Không tìm thấy thử thách");
    }
}
