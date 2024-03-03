using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Repository.Repositories;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Unibean.API.Filters;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Unibean.API.Swaggers;
using MoreLinq;
using Unibean.API.Background;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// JWT authentication service
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration
                    ["Jwt:Key"]))
    };
});

builder.Services.AddAuthorization();

// Configure exception handler
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Controller service, exception handler
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ExceptionFilter>();
});

// Configure Background Service
builder.Services.AddHostedService<BackgroundWorkerService>();

// Configure Date/Datetime/Timeonly parameter
builder.Services.AddDateOnlyTimeOnlyStringConverters();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder.AllowAnyOrigin());
});

// Configure swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "📗Unibean REST API", Version = "v1" });
    c.OrderActionsBy(apiDesc => apiDesc.RelativePath);
    c.UseDateOnlyTimeOnlyStringConverters();
    c.OperationFilter<AuthorizationOperationFilter>();

    // using System.Reflection
    string[] xmlCommentFileNames =
    {
    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml",
    "Unibean.Service.xml"
    };
    xmlCommentFileNames.ForEach(fileName
        => c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, fileName)));
});
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Dependency injection service
// Repository
builder.Services.AddSingleton<IAccountRepository, AccountRepository>();
builder.Services.AddSingleton<IActivityRepository, ActivityRepository>();
builder.Services.AddSingleton<IActivityTransactionRepository, ActivityTransactionRepository>();
builder.Services.AddSingleton<IAdminRepository, AdminRepository>();
builder.Services.AddSingleton<IAreaRepository, AreaRepository>();
builder.Services.AddSingleton<IBonusRepository, BonusRepository>();
builder.Services.AddSingleton<IBonusTransactionRepository, BonusTransactionRepository>();
builder.Services.AddSingleton<IBrandRepository, BrandRepository>();
builder.Services.AddSingleton<ICampaignActivityRepository, CampaignActivityRepository>();
builder.Services.AddSingleton<ICampaignCampusRepository, CampaignCampusRepository>();
builder.Services.AddSingleton<ICampaignDetailRepository, CampaignDetailRepository>();
builder.Services.AddSingleton<ICampaignMajorRepository, CampaignMajorRepository>();
builder.Services.AddSingleton<ICampaignRepository, CampaignRepository>();
builder.Services.AddSingleton<ICampaignStoreRepository, CampaignStoreRepository>();
builder.Services.AddSingleton<ICampaignTransactionRepository, CampaignTransactionRepository>();
builder.Services.AddSingleton<ICampaignTypeRepository, CampaignTypeRepository>();
builder.Services.AddSingleton<ICampusRepository, CampusRepository>();
builder.Services.AddSingleton<ICategoryRepository, CategoryRepository>();
builder.Services.AddSingleton<IChallengeRepository, ChallengeRepository>();
builder.Services.AddSingleton<IChallengeTransactionRepository, ChallengeTransactionRepository>();
builder.Services.AddSingleton<IImageRepository, ImageRepository>();
builder.Services.AddSingleton<IInvitationRepository, InvitationRepository>();
builder.Services.AddSingleton<IMajorRepository, MajorRepository>();
builder.Services.AddSingleton<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<IOrderStateRepository, OrderStateRepository>();
builder.Services.AddSingleton<IOrderTransactionRepository, OrderTransactionRepository>();
builder.Services.AddSingleton<IProductRepository, ProductRepository>();
builder.Services.AddSingleton<IRequestRepository, RequestRepository>();
builder.Services.AddSingleton<IRequestTransactionRepository, RequestTransactionRepository>();
builder.Services.AddSingleton<IStaffRepository, StaffRepository>();
builder.Services.AddSingleton<IStationRepository, StationRepository>();
builder.Services.AddSingleton<IStoreRepository, StoreRepository>();
builder.Services.AddSingleton<IStudentChallengeRepository, StudentChallengeRepository>();
builder.Services.AddSingleton<IStudentRepository, StudentRepository>();
builder.Services.AddSingleton<IUniversityRepository, UniversityRepository>();
builder.Services.AddSingleton<IVoucherItemRepository, VoucherItemRepository>();
builder.Services.AddSingleton<IVoucherRepository, VoucherRepository>();
builder.Services.AddSingleton<IVoucherTypeRepository, VoucherTypeRepository>();
builder.Services.AddSingleton<IWalletRepository, WalletRepository>();
builder.Services.AddSingleton<IWishlistRepository, WishlistRepository>();

// Service
builder.Services.AddSingleton<IAccountService, AccountService>();
builder.Services.AddSingleton<IActivityService, ActivityService>();
builder.Services.AddSingleton<IActivityTransactionService, ActivityTransactionService>();
builder.Services.AddSingleton<IAdminService, AdminService>();
builder.Services.AddSingleton<IAreaService, AreaService>();
builder.Services.AddSingleton<IBonusService, BonusService>();
builder.Services.AddSingleton<IBonusTransactionService, BonusTransactionService>();
builder.Services.AddSingleton<IBrandService, BrandService>();
builder.Services.AddSingleton<ICampaignActivityService, CampaignActivityService>();
builder.Services.AddSingleton<ICampaignCampusService, CampaignCampusService>();
builder.Services.AddSingleton<ICampaignDetailService, CampaignDetailService>();
builder.Services.AddSingleton<ICampaignMajorService, CampaignMajorService>();
builder.Services.AddSingleton<ICampaignService, CampaignService>();
builder.Services.AddSingleton<ICampaignStoreService, CampaignStoreService>();
builder.Services.AddSingleton<ICampaignTransactionService, CampaignTransactionService>();
builder.Services.AddSingleton<ICampaignTypeService, CampaignTypeService>();
builder.Services.AddSingleton<ICampusService, CampusService>();
builder.Services.AddSingleton<ICategoryService, CategoryService>();
builder.Services.AddSingleton<IChallengeService, ChallengeService>();
builder.Services.AddSingleton<IChallengeTransactionService, ChallengeTransactionService>();
builder.Services.AddSingleton<IImageService, ImageService>();
builder.Services.AddSingleton<IInvitationService, InvitationService>();
builder.Services.AddSingleton<IMajorService, MajorService>();
builder.Services.AddSingleton<IOrderDetailService, OrderDetailService>();
builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddSingleton<IOrderStateService, OrderStateService>();
builder.Services.AddSingleton<IOrderTransactionService, OrderTransactionService>();
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IRequestService, RequestService>();
builder.Services.AddSingleton<IRequestTransactionService, RequestTransactionService>();
builder.Services.AddSingleton<IStaffService, StaffService>();
builder.Services.AddSingleton<IStationService, StationService>();
builder.Services.AddSingleton<IStoreService, StoreService>();
builder.Services.AddSingleton<IStudentChallengeService, StudentChallengeService>();
builder.Services.AddSingleton<IStudentService, StudentService>();
builder.Services.AddSingleton<IUniversityService, UniversityService>();
builder.Services.AddSingleton<IVoucherItemService, VoucherItemService>();
builder.Services.AddSingleton<IVoucherService, VoucherService>();
builder.Services.AddSingleton<IVoucherTypeService, VoucherTypeService>();
builder.Services.AddSingleton<IWalletService, WalletService>();
builder.Services.AddSingleton<IWishlistService, WishlistService>();

// FireBase
builder.Services.AddSingleton<IFireBaseService, FireBaseService>();

// Google
builder.Services.AddSingleton<IGoogleService, GoogleService>();

// Email
builder.Services.AddSingleton<IEmailService, EmailService>();

// Jwt
builder.Services.AddSingleton<IJwtService, JwtService>();

// Discord
builder.Services.AddSingleton<IDiscordService, DiscordService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c
    =>
{
    c.InjectStylesheet("/css/swagger/swagger-ui.css");
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Unibean REST API v1");
});

app.UseStaticFiles();
app.UseRouting();

app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
