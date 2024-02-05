using AutoMapper;
using Enable.EnumDisplayName;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.StudentChallenges;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class StudentChallengeService : IStudentChallengeService
{
    private readonly Mapper mapper;

    private readonly IStudentChallengeRepository studentChallengeRepository;

    public StudentChallengeService(IStudentChallengeRepository studentChallengeRepository)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<StudentChallenge, StudentChallengeModel>()
            .ForMember(c => c.StudentName, opt => opt.MapFrom(src => src.Student.FullName))
            .ForMember(c => c.ChallengeTypeId, opt => opt.MapFrom(src => (int)src.Challenge.Type))
            .ForMember(c => c.ChallengeType, opt => opt.MapFrom(src => src.Challenge.Type))
            .ForMember(c => c.ChallengeTypeName, opt => opt.MapFrom(src => src.Challenge.Type.GetDisplayName()))
            .ForMember(c => c.ChallengeName, opt => opt.MapFrom(src => src.Challenge.ChallengeName))
            .ForMember(c => c.IsClaimed, opt => opt.MapFrom(src => src.ChallengeTransactions.Any()))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<StudentChallenge>, PagedResultModel<StudentChallengeModel>>()
            .ReverseMap();
            cfg.CreateMap<StudentChallenge, CreateStudentChallengeModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.studentChallengeRepository = studentChallengeRepository;
    }

    public StudentChallengeModel Add(CreateStudentChallengeModel creation)
    {
        return mapper.Map<StudentChallengeModel>(studentChallengeRepository
            .Add(mapper.Map<StudentChallenge>(creation)));
    }

    public void Delete(string id)
    {
        StudentChallenge entity = studentChallengeRepository.GetById(id);
        if (entity != null)
        {
            studentChallengeRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy thử thách của sinh viên");
        }
    }

    public PagedResultModel<StudentChallengeModel> GetAll
        (List<string> studentIds, List<string> challengeIds, List<ChallengeType> typeIds,
        bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<StudentChallengeModel>>(studentChallengeRepository
            .GetAll(studentIds, challengeIds, typeIds, state, propertySort, isAsc, search, page, limit));
    }

    public StudentChallengeModel GetById(string id)
    {
        StudentChallenge entity = studentChallengeRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<StudentChallengeModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy thử thách của sinh viên");
    }

    public void Update(IEnumerable<StudentChallenge> studentChallenges, decimal amount)
    {
        foreach (var challenge in studentChallenges)
        {
            challenge.Current += amount;
            challenge.IsCompleted = challenge.Current >= challenge.Condition;
            challenge.DateUpdated = DateTime.Now;
            studentChallengeRepository.Update(challenge);
        }
    }
}
