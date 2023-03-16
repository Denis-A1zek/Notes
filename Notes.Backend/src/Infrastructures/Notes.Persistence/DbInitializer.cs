namespace Notes.Persistence;

public class DbInitializer
{
    public static void Initializer(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();
    }
}
