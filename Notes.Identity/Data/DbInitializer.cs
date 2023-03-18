namespace Notes.Identity.Data;

public class DbInitializer
{
    public static void Initializer(ApplicationDbContext context)
    {
        context.Database.EnsureCreated();
    }
}
