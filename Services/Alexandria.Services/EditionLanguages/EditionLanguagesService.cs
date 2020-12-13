namespace Alexandria.Services.EditionLanguages
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class EditionLanguagesService : IEditionLanguagesService
    {
        private readonly AlexandriaDbContext db;

        public EditionLanguagesService(AlexandriaDbContext db)
        {
            this.db = db;
        }

        public async Task<bool> DoesEditionLanguageIdExistAsync(int id)
            => await this.db.EditionLanguages.AnyAsync(l => l.Id == id);

        public async Task<IEnumerable<TModel>> GetAllLanguagesAsync<TModel>()
        {
            var languages = await this.db.EditionLanguages.AsNoTracking()
                                                          .OrderBy(l => l.Name)
                                                          .To<TModel>()
                                                          .ToListAsync();

            return languages;
        }
    }
}
