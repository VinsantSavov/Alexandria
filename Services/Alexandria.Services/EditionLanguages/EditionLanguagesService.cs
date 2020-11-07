namespace Alexandria.Services.EditionLanguages
{
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    public class EditionLanguagesService : IEditionLanguagesService
    {
        private readonly AlexandriaDbContext db;
        private readonly IMapper mapper;

        public EditionLanguagesService(AlexandriaDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task CreateEditionLanguageAsync(string name)
        {
            var language = new EditionLanguage
            {
                Name = name,
            };

            await this.db.EditionLanguages.AddAsync(language);
            await this.db.SaveChangesAsync();
        }

        public async Task<TModel> GetEditionLanguageByIdAsync<TModel>(int id)
        {
            var language = await this.db.EditionLanguages.Where(l => l.Id == id)
                                                   .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                                                   .FirstOrDefaultAsync();

            return language;
        }
    }
}
