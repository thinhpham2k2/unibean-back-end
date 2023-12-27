using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.States;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class StateService : IStateService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "states";

    private readonly IStateRepository stateRepository;

    private readonly IFireBaseService fireBaseService;

    public StateService(IStateRepository stateRepository, 
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<State, StateModel>()
            .ForMember(t => t.State, opt => opt.MapFrom(src => src.States))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<State>, PagedResultModel<StateModel>>()
            .ReverseMap();
            cfg.CreateMap<State, UpdateStateModel>()
            .ReverseMap()
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.States, opt => opt.MapFrom(src => src.State));
            cfg.CreateMap<State, CreateStateModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.States, opt => opt.MapFrom(src => src.State))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.stateRepository = stateRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<StateModel> Add(CreateStateModel creation)
    {
        State entity = mapper.Map<State>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<StateModel>(stateRepository.Add(entity));
    }

    public void Delete(string id)
    {
        State entity = stateRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Image != null && entity.FileName != null)
            {
                //Remove image
                fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
            }
            stateRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Not found state");
        }
    }

    public PagedResultModel<StateModel> GetAll(string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<StateModel>>(stateRepository.GetAll(propertySort, isAsc, search, page, limit));
    }

    public StateModel GetById(string id)
    {
        State entity = stateRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<StateModel>(entity);
        }
        throw new InvalidParameterException("Not found state");
    }

    public async Task<StateModel> Update(string id, UpdateStateModel update)
    {
        State entity = stateRepository.GetById(id);
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
            return mapper.Map<StateModel>(stateRepository.Update(entity));
        }
        throw new InvalidParameterException("Not found state");
    }
}
