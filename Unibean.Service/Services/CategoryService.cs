﻿using AutoMapper;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Categories;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class CategoryService : ICategoryService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "categories";

    private readonly ICategoryRepository categoryRepository;

    private readonly IFireBaseService fireBaseService;

    public CategoryService(ICategoryRepository categoryRepository,
        IFireBaseService fireBaseService)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Category, CategoryModel>()
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Category>, PagedResultModel<CategoryModel>>()
            .ReverseMap();
            cfg.CreateMap<Category, CategoryExtraModel>()
            .ForMember(c => c.NumberOfProducts, opt => opt.MapFrom(src => src.Products.Count))
            .ReverseMap();
            cfg.CreateMap<Category, UpdateCategoryModel>()
            .ReverseMap()
            .ForMember(t => t.Image, opt => opt.Ignore())
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
            cfg.CreateMap<Category, CreateCategoryModel>()
            .ReverseMap()
            .ForMember(t => t.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(t => t.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(t => t.Status, opt => opt.MapFrom(src => true));
        });
        mapper = new Mapper(config);
        this.categoryRepository = categoryRepository;
        this.fireBaseService = fireBaseService;
    }

    public async Task<CategoryExtraModel> Add(CreateCategoryModel creation)
    {
        Category entity = mapper.Map<Category>(creation);

        //Upload image
        if (creation.Image != null && creation.Image.Length > 0)
        {
            FireBaseFile f = await fireBaseService.UploadFileAsync(creation.Image, FOLDER_NAME);
            entity.Image = f.URL;
            entity.FileName = f.FileName;
        }
        return mapper.Map<CategoryExtraModel>(categoryRepository.Add(entity));
    }

    public void Delete(string id)
    {
        Category entity = categoryRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Products.Count.Equals(0))
            {
                if (entity.Image != null && entity.FileName != null)
                {
                    //Remove image
                    fireBaseService.RemoveFileAsync(entity.FileName, FOLDER_NAME);
                }
                categoryRepository.Delete(id);
            }
            else
            {
                throw new InvalidParameterException("Xóa thất bại do tồn tại sản phẩm thuộc thể loại");
            }
        }
        else
        {
            throw new InvalidParameterException("Không tìm thấy thể loại sản phẩm");
        }
    }

    public PagedResultModel<CategoryModel> GetAll
        (bool? state, string propertySort, bool isAsc, string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<CategoryModel>>(
            categoryRepository.GetAll(state, propertySort, isAsc, search, page, limit));
    }

    public CategoryExtraModel GetById(string id)
    {
        Category entity = categoryRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<CategoryExtraModel>(entity);
        }
        throw new InvalidParameterException("Không tìm thấy thể loại sản phẩm");
    }

    public async Task<CategoryExtraModel> Update(string id, UpdateCategoryModel update)
    {
        Category entity = categoryRepository.GetById(id);
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
            return mapper.Map<CategoryExtraModel>(categoryRepository.Update(entity));
        }
        throw new InvalidParameterException("Không tìm thấy thể loại sản phẩm");
    }
}
