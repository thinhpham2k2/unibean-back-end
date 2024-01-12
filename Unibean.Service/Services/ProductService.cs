using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using MoreLinq;
using Unibean.Repository.Entities;
using Unibean.Repository.Paging;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Models.Exceptions;
using Unibean.Service.Models.Products;
using Unibean.Service.Services.Interfaces;
using Unibean.Service.Utilities.FireBase;

namespace Unibean.Service.Services;

public class ProductService : IProductService
{
    private readonly Mapper mapper;

    private readonly string FOLDER_NAME = "products";

    private readonly IProductRepository productRepository;

    private readonly IFireBaseService fireBaseService;

    private readonly IImageRepository imageRepository;

    public ProductService(IProductRepository productRepository,
        IFireBaseService fireBaseService,
        IImageRepository imageRepository)
    {
        var config = new MapperConfiguration(cfg
                =>
        {
            cfg.CreateMap<Product, ProductModel>()
            .ForMember(p => p.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
            .ForMember(p => p.ProductImage, opt => opt.MapFrom(src => src.Images.Where(i
                => (bool)i.IsCover).FirstOrDefault().Url))
            .ReverseMap();
            cfg.CreateMap<PagedResultModel<Product>, PagedResultModel<ProductModel>>()
            .ReverseMap();
            cfg.CreateMap<Product, ProductExtraModel>()
            .ForMember(p => p.CategoryName, opt => opt.MapFrom(src => src.Category.CategoryName))
            .ForMember(p => p.CategoryImage, opt => opt.MapFrom(src => src.Category.Image))
            .ForMember(p => p.ProductImages, opt => opt.MapFrom(src => src.Images.Select(i => i.Url).ToList()))
            .ForMember(p => p.NumOfSold, opt => opt.MapFrom(src => src.OrderDetails.Count))
            .ReverseMap();
            cfg.CreateMap<Product, CreateProductModel>()
            .ReverseMap()
            .ForMember(p => p.Id, opt => opt.MapFrom(src => Ulid.NewUlid()))
            .ForMember(p => p.DateCreated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.DateUpdated, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(p => p.Status, opt => opt.MapFrom(src => true));
            cfg.CreateMap<Product, UpdateProductModel>()
            .ReverseMap()
            .ForMember(t => t.Category, opt => opt.MapFrom(src => (string)null))
            .ForMember(p => p.Images, opt => opt.MapFrom(src => (string)null))
            .ForMember(p => p.DateUpdated, opt => opt.MapFrom(src => DateTime.Now));
        });
        mapper = new Mapper(config);
        this.productRepository = productRepository;
        this.fireBaseService = fireBaseService;
        this.imageRepository = imageRepository;
    }

    public async Task<ProductModel> Add(CreateProductModel creation)
    {
        Product entity = mapper.Map<Product>(creation);
        entity = productRepository.Add(entity);

        bool isCover = true;
        if (creation.ProductImages != null)
        {
            foreach (var image in creation.ProductImages)
            {
                //Upload image
                if (image != null && image.Length > 0)
                {
                    FireBaseFile f = await fireBaseService.UploadFileAsync(image, FOLDER_NAME + "/" + entity.Id);
                    imageRepository.Add(new Image
                    {
                        Id = Ulid.NewUlid().ToString(),
                        ProductId = entity.Id,
                        Url = f.URL,
                        FileName = f.FileName,
                        IsCover = isCover,
                        DateCreated = DateTime.Now,
                        State = true,
                        Status = true
                    });
                    isCover = false;
                }
            }
        }

        return mapper.Map<ProductModel>(entity);
    }

    public void Delete(string id)
    {
        Product entity = productRepository.GetById(id);
        if (entity != null)
        {
            if (entity.Images != null)
            {
                foreach (var image in entity.Images)
                {
                    if (image.Url != null && image.FileName != null)
                    {
                        //Remove image
                        fireBaseService.RemoveFileAsync(image.FileName, FOLDER_NAME + "/" + entity.Id);
                    }
                    imageRepository.Delete(image.Id);
                }
            }
            productRepository.Delete(id);
        }
        else
        {
            throw new InvalidParameterException("Not found product");
        }
    }

    public PagedResultModel<ProductModel> GetAll
        (List<string> categoryIds, string propertySort, bool isAsc,
        string search, int page, int limit)
    {
        return mapper.Map<PagedResultModel<ProductModel>>(productRepository.GetAll
            (categoryIds, propertySort, isAsc, search, page, limit));
    }

    public ProductExtraModel GetById(string id)
    {
        Product entity = productRepository.GetById(id);
        if (entity != null)
        {
            return mapper.Map<ProductExtraModel>(entity);
        }
        throw new InvalidParameterException("Not found product");
    }

    public async Task<ProductExtraModel> Update(string id, UpdateProductModel update)
    {
        Product entity = productRepository.GetById(id);
        if (entity != null)
        {
            if (update.ProductImages != null && entity.Images != null)
            {
                foreach (var image in entity.Images)
                {
                    // Remove image
                    if (!image.Url.IsNullOrEmpty() && !image.FileName.IsNullOrEmpty())
                    {
                        await fireBaseService.RemoveFileAsync(image.FileName, FOLDER_NAME + "/" + entity.Id);
                    }
                    imageRepository.Delete(image.Id);
                }
            }

            entity = mapper.Map(update, entity);

            if (update.ProductImages != null)
            {
                bool isCover = true;
                foreach (var image in update.ProductImages)
                {
                    //Upload image
                    if (image != null && image.Length > 0)
                    {
                        FireBaseFile f = await fireBaseService.UploadFileAsync(image, FOLDER_NAME + "/" + entity.Id);
                        imageRepository.Add(new Image
                        {
                            Id = Ulid.NewUlid().ToString(),
                            ProductId = entity.Id,
                            Url = f.URL,
                            FileName = f.FileName,
                            IsCover = isCover,
                            DateCreated = DateTime.Now,
                            State = true,
                            Status = true
                        });
                        isCover = false;
                    }
                }
            }

            return mapper.Map<ProductExtraModel>(productRepository.Update(entity));
        }
        throw new InvalidParameterException("Not found product");
    }
}
