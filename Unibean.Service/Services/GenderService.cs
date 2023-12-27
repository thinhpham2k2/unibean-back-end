using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Genders;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class GenderService : IGenderService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "genders";

    private readonly IGenderRepository genderRepository;

    private readonly IFireBaseService fireBaseService;

    public GenderService(IGenderRepository genderRepository,
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Gender, GenderModel>().ReverseMap();
            cfg.CreateMap<PagedResultModel<Gender>, PagedResultModel<GenderModel>>()
            .ReverseMap();
            cfg.CreateMap<Gender, UpdateGenderModel>()
            .ReverseMap()
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<Gender, CreateGenderModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.genderRepository = genderRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<GenderModel> Add(CreateGenderModel creation)
    {
        Gender entity = mapper.Map<Gender>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<GenderModel>(genderRepository.Add(entity));
    }

    public void Delete(string id)
    {
        Gender entity = genderRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Image != null && entity.FileName != null)
            {
                //Remove image
                fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
            }
            genderRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Not found gender");
        }
    }

    public PagedResultModel<GenderModel> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<GenderModel>>(genderRepository.GetAll(propertySort, isAsc, search, page, limit));
    }

    public GenderModel GetById(string id)
    {
        Gender entity = genderRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<GenderModel>(entity);
        }
        throw new InvalidParameterException("Not found gender");
    }

    public async Task<GenderModel> Update(string id, UpdateGenderModel update)
    {
        Gender entity = genderRepository.GetById(id);
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
            return mapper.Map<GenderModel>(genderRepository.Update(entity));
        }
        throw new InvalidParameterException("Not found gender");
    }
}
