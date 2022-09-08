namespace Ejournal.Identity.Data
{
    public static class DbInitializer
    {
        public static void Initializee(AuthDbContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}
