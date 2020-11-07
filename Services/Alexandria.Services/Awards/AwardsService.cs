namespace Alexandria.Services.Awards
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Common;
    using Alexandria.Services.Mapping;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    public class AwardsService : IAwardsService
    {
        private readonly AlexandriaDbContext db;
        private readonly IMapper mapper;

        public AwardsService(AlexandriaDbContext db)
        {
            this.db = db;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        public async Task CreateAwardAsync(string name)
        {
            var award = new Award
            {
                Name = name,
            };

            await this.db.Awards.AddAsync(award);
        }

        public async Task DeleteAwardByIdAsync(int id)
        {
            var award = await this.db.Awards.FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

            if (award == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.AwardNotFound, id));
            }

            award.IsDeleted = true;
            award.DeletedOn = DateTime.UtcNow;

            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllAwardsAsync<TModel>()
        {
            var awards = await this.db.Awards.AsNoTracking()
                                       .Where(a => !a.IsDeleted)
                                       .OrderBy(a => a.Name)
                                       .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                                       .ToListAsync();

            return awards;
        }
    }
}
