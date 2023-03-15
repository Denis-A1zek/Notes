using Notes.Persistence;

namespace Notes.WebApi.Definitions.DbContext;

public static class DbInitializerDefinition
{
    public static WebApplication UseDbInitializer(this WebApplication app)
    {
        using(var scope = app.Services.CreateScope())
        {
            var serviceProvider = scope.ServiceProvider;
            try
            {
                var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                DbInitializer.Initializer(context);
            }
            catch (Exception ex) { }
        }

        return app;
    }
}
