using Application.Helpers.Hubs;

namespace Web.Extensions;

internal static class RegisterAppsExtension
{
    internal static void Use(this WebApplication app)
    {
        // Configure the HTTP request pipeline.

        #region Initialization of Database
        IDatabaseInitializer dbInitializer = app.Services.CreateScope().ServiceProvider.GetRequiredService<IDatabaseInitializer>();
        dbInitializer.MigrateDbsAsync().Wait();
        dbInitializer.SeedDataAsync().Wait();
        #endregion

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseRouting();
        app.UseStaticFiles();
        app.UseMiddleware<BackgroundJobMiddleware>();

        #region Using Hangfire.
        app.UseHangfireDashboard();
        #endregion

        app.UseHttpsRedirection();
        app.UseAuthorization();

        #region Using middleware for handling global exception.
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        #endregion

        #region Using rate limiter.
        app.UseRateLimiter();
        #endregion

        #region Using CORS.
        app.UseCors();
        #endregion

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<NotificationHub>("notificationHub");
        });

        app.Run();
    }
}
