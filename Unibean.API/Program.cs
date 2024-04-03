using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoreLinq;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using System.Text;
using Unibean.API.Backgrounds;
using Unibean.API.Filters;
using Unibean.API.Swaggers;
using Unibean.Repository.Entities;
using Unibean.Repository.Repositories;
using Unibean.Repository.Repositories.Interfaces;
using Unibean.Service.Services;
using Unibean.Service.Services.Interfaces;

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
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IActivityTransactionRepository, ActivityTransactionRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IBonusRepository, BonusRepository>();
builder.Services.AddScoped<IBonusTransactionRepository, BonusTransactionRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ICampaignActivityRepository, CampaignActivityRepository>();
builder.Services.AddScoped<ICampaignCampusRepository, CampaignCampusRepository>();
builder.Services.AddScoped<ICampaignDetailRepository, CampaignDetailRepository>();
builder.Services.AddScoped<ICampaignMajorRepository, CampaignMajorRepository>();
builder.Services.AddScoped<ICampaignRepository, CampaignRepository>();
builder.Services.AddScoped<ICampaignStoreRepository, CampaignStoreRepository>();
builder.Services.AddScoped<ICampaignTransactionRepository, CampaignTransactionRepository>();
builder.Services.AddScoped<ICampaignTypeRepository, CampaignTypeRepository>();
builder.Services.AddScoped<ICampusRepository, CampusRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IChallengeRepository, ChallengeRepository>();
builder.Services.AddScoped<IChallengeTransactionRepository, ChallengeTransactionRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();
builder.Services.AddScoped<IMajorRepository, MajorRepository>();
builder.Services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderStateRepository, OrderStateRepository>();
builder.Services.AddScoped<IOrderTransactionRepository, OrderTransactionRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IRequestTransactionRepository, RequestTransactionRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IStationRepository, StationRepository>();
builder.Services.AddScoped<IStoreRepository, StoreRepository>();
builder.Services.AddScoped<IStudentChallengeRepository, StudentChallengeRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUniversityRepository, UniversityRepository>();
builder.Services.AddScoped<IVoucherItemRepository, VoucherItemRepository>();
builder.Services.AddScoped<IVoucherRepository, VoucherRepository>();
builder.Services.AddScoped<IVoucherTypeRepository, VoucherTypeRepository>();
builder.Services.AddScoped<IWalletRepository, WalletRepository>();
builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();

// Service
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IActivityService, ActivityService>();
builder.Services.AddScoped<IActivityTransactionService, ActivityTransactionService>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<IBonusService, BonusService>();
builder.Services.AddScoped<IBonusTransactionService, BonusTransactionService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICampaignActivityService, CampaignActivityService>();
builder.Services.AddScoped<ICampaignCampusService, CampaignCampusService>();
builder.Services.AddScoped<ICampaignDetailService, CampaignDetailService>();
builder.Services.AddScoped<ICampaignMajorService, CampaignMajorService>();
builder.Services.AddScoped<ICampaignService, CampaignService>();
builder.Services.AddScoped<ICampaignStoreService, CampaignStoreService>();
builder.Services.AddScoped<ICampaignTransactionService, CampaignTransactionService>();
builder.Services.AddScoped<ICampaignTypeService, CampaignTypeService>();
builder.Services.AddScoped<ICampusService, CampusService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IChallengeService, ChallengeService>();
builder.Services.AddScoped<IChallengeTransactionService, ChallengeTransactionService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IInvitationService, InvitationService>();
builder.Services.AddScoped<IMajorService, MajorService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderStateService, OrderStateService>();
builder.Services.AddScoped<IOrderTransactionService, OrderTransactionService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IRequestTransactionService, RequestTransactionService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IStationService, StationService>();
builder.Services.AddScoped<IStoreService, StoreService>();
builder.Services.AddScoped<IStudentChallengeService, StudentChallengeService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IUniversityService, UniversityService>();
builder.Services.AddScoped<IVoucherItemService, VoucherItemService>();
builder.Services.AddScoped<IVoucherService, VoucherService>();
builder.Services.AddScoped<IVoucherTypeService, VoucherTypeService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();

// FireBase
builder.Services.AddScoped<IFireBaseService, FireBaseService>();

// Google
builder.Services.AddScoped<IGoogleService, GoogleService>();

// Email
builder.Services.AddScoped<IEmailService, EmailService>();

// Jwt
builder.Services.AddScoped<IJwtService, JwtService>();

// Discord
builder.Services.AddScoped<IDiscordService, DiscordService>();

// Chart
builder.Services.AddScoped<IChartService, ChartService>();

// Database
builder.Services.AddDbContext<UnibeanDBContext>();

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
