﻿using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Universities;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class UniversityService : IUniversityService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "universities";

    private readonly IUniversityRepository universityRepository;

    private readonly IFireBaseService fireBaseService;

    public UniversityService(IUniversityRepository universityRepository,
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<University, UniversityModel>()
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<University>, PagedResultModel<UniversityModel>>()
            .ReverseMap();
            cfg.CreateMap<University, UniversityExtraModel>()
            .ForMember(u => u.NumberOfCampuses, opt => opt.MapFrom(src => src.Campuses.Count))
            .ForMember(u => u.NumberOfStudents, opt => opt.MapFrom(src => src.Campuses.Select(c => c.Students.Count).Sum()))
            .ReverseMap();
            cfg.CreateMap<University, UpdateUniversityModel>()
            .ReverseMap()
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<University, CreateUniversityModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.universityRepository = universityRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<UniversityExtraModel> Add(CreateUniversityModel creation)
    {
        University entity = mapper.Map<University>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<UniversityExtraModel>(universityRepository.Add(entity));
    }

    public void Delete(string id)
    {
        University entity = universityRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Campuses.Count.Equals(0))
            {
                if (entity.Image != null && entity.FileName != null)
                {
                    //Remove image
                    fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
                }
                universityRepository.Delete(id);
            }
            else
            {
                throw new InvalidParameterException("Xóa thất bại do tồn tại cơ sở thuộc trường đại học");
            }
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy trường đại học");
        }
    }

    public PagedResultModel<UniversityModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<UniversityModel>>
            (universityRepository.GetAll(state, propertySort, isAsc, search, page, limit));
    }

    public UniversityExtraModel GetById(string id)
    {
        University entity = universityRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<UniversityExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy trường đại học");
    }

    public async Task<UniversityExtraModel> Update(string id, UpdateUniversityModel update)
    {
        University entity = universityRepository.GetById(id);
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
            return mapper.Map<UniversityExtraModel>(universityRepository.Update(entity));
        }
        throw new InvalidParameterException("Không tìm thấy trường đại học");
    }
}
