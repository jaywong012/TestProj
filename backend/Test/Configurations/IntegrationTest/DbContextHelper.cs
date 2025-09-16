using Infrastructure.Configurations;

namespace Test.Configurations.IntegrationTest;

public static class DbContextHelper
{
    public static void ClearEntities<T>(NewProjectDbContext context) where T : class
    {
        var dbSet = context.Set<T>();
        var entities = dbSet.ToList();
        dbSet.RemoveRange(entities);
        context.SaveChangesAsync();
    }
}