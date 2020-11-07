namespace Alexandria.Data.Seeding
{
    using System;
    using System.Threading.Tasks;

    public interface ISeeder
    {
        Task SeedAsync(AlexandriaDbContext dbContext, IServiceProvider serviceProvider);
    }
}
