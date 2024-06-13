using Application.Abstractions.Services;
using Infrastructure.Services;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfigurationRoot configuration)
    {
        var connectionString = configuration.GetConnectionString("DBConnection");
        services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(connectionString, op =>
        {
            op.CommandTimeout(120);
        }));
        services.AddIdentity<User, Roles>(option =>
        {
            option.User.RequireUniqueEmail = true;
            option.Password.RequireDigit = false;
            option.Password.RequiredLength = 8;
            option.Password.RequireNonAlphanumeric = true;
            option.Password.RequireUppercase = false;
            option.Password.RequireLowercase = false;
            option.SignIn.RequireConfirmedEmail = true;
        }).AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        services.AddScoped<IDatabaseInitializer, DatabaseInitializer>();

        services.AddAuthentication(auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
#pragma warning disable CS8604 // Possible null reference argument.
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration["AuthSettings:Audience"],
                ValidIssuer = configuration["AuthSettings:Issuer"],
                RequireExpirationTime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["AuthSettings:Key"])),
                ValidateIssuerSigningKey = true
            };
#pragma warning restore CS8604 // Possible null reference argument.
        });

        #region Repositories
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        #endregion

        #region Registered UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        #endregion

        #region Services
        services.AddScoped<IMailService, MailService>();
        #endregion

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(ApplicationRegisteration).Assembly));

        #region File Service
        services.AddTransient<IFileHelper, FileHelper>();
        services.AddTransient<INotificationService, NotificationService>();
        #endregion

        #region Register automapper
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        #endregion

        return services;
    }
}