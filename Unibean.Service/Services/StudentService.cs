using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Students;
using Unibean.Service.Services.Interfaces;

namespace Unibean.Service.Services;

public class StudentService : IStudentService
{
    private readonly Mapper mapper;

    private readonly IStudentRepository studentRepository;

    private readonly IFireBaseService fireBaseService;

    public StudentService(IStudentRepository studentRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Student, StudentModel>()
            .ForMember(s => s.LevelName, opt => opt.MapFrom(src => src.Level.LevelName))
            .ForMember(s => s.GenderName, opt => opt.MapFrom(src => src.Gender.GenderName))
            .ForMember(s => s.MajorName, opt => opt.MapFrom(src => src.Major.MajorName))
            .ForMember(s => s.CampusName, opt => opt.MapFrom(src => src.Campus.CampusName))
            .ReverseMap();
        });
        mapper = new Mapper(config);
        this.studentRepository = studentRepository;
        this.fireBaseService = fireBaseService;
    }

    public StudentModel GetByUserNameAndPassword(string userName, string password)
    {
        StudentModel student = mapper.Map<StudentModel>(studentRepository.GetByUserNameAndPassword(userName, password));
        return student != null ? 
            (bool)student.IsVerify ? student : throw new InvalidParameterException("Account is not verified")
            : null;
    }
}
