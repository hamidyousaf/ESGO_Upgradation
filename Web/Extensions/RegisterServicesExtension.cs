using Application.Helpers;
using Wkhtmltopdf.NetCore;

namespace Web.Extensions;
internal static class RegisterServicesExtension
{
    public static IServiceCollection Register(this IServiceCollection services, ConfigurationManager _configuration)
    {
        // Add services to the container.
        var configuration = _configuration
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
            .Build();

        #region For MediatR.
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        #endregion

        #region For Swagger
        services
              .AddSwaggerGen(c =>
              {
                  c.AddSecurityDefinition("Bearer", //Name the security scheme
                  new OpenApiSecurityScheme
                  {
                      Description = "JWT Authorization header using the Bearer scheme.",
                      Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                      Scheme = "bearer" //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                  });

                  c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                 {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "Bearer", //The name of the previously defined security scheme.
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });
                  c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
              });
        #endregion 

        #region For Infrastructure project
        // Adding from Infrastructure project. All repos will be register there.
        services.AddInfrastructure(configuration);
        #endregion

        services.AddControllers();
        services.AddSingleton<PresenceTracker>();
        services.AddSignalR();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        #region For Middlewares
        services.AddTransient<ExceptionHandlingMiddleware>();
        #endregion

        #region For Rate Limiting
        // Rate limiting is a technique for restricting the number of requests to our API.
        // Rate limiting is a .Net 7 feature.
        // If we set QueueLimit to 0, it shows 503 error.
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddFixedWindowLimiter("fixedPolicy", opt =>
            {
                opt.Window = TimeSpan.FromSeconds(10);
                opt.PermitLimit = 1;
                opt.QueueLimit = 2;
                opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
            });

            options.AddSlidingWindowLimiter("slidingPolicy", opt =>
            {
                opt.Window = TimeSpan.FromSeconds(30);
                opt.PermitLimit = 3;
                opt.SegmentsPerWindow = 3;
            });

            options.AddConcurrencyLimiter("concurrencyPolicy", opt =>
            {
                opt.PermitLimit = 5;
            });

            options.AddTokenBucketLimiter("tokenPolicy", opt =>
            {
                opt.TokenLimit = 10;
                opt.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
                opt.TokensPerPeriod = 2;
            });
        });

        #endregion

        #region For Hangfire
        // Add Hangfire services.
        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(_configuration.GetConnectionString("DBConnection")));

        // Add the processing server as IHostedService
        services.AddHangfireServer();

        services.AddTransient<BackgroundJobMiddleware>();
        #endregion

        services.AddWkhtmltopdf("wkhtmltopdf");  //name added from the wkhtmltopdf.exe file
        #region Register Cors Policy
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowCredentials()
                       .AllowAnyHeader();
            });
        });
        #endregion

        return services;
    }
}
