namespace Alexandria.Services.Awards
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class AwardsService : IAwardsService
    {
        private readonly AlexandriaDbContext db;

        public AwardsService(AlexandriaDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> DoesAwardIdExistAsync(int id)
            => await this.db.Awards.AnyAsync(a => a.Id == id && !a.IsDeleted);

        public async Task<IEnumerable<TModel>> GetAllAwardsAsync<TModel>()
        {
            var awards = await this.db.Awards.AsNoTracking()
                                       .Where(a => !a.IsDeleted)
                                       .OrderBy(a => a.Name)
                                       .To<TModel>()
                                       .ToListAsync();

            return awards;
        }
    }
}
