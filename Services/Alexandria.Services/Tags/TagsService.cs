namespace Alexandria.Services.Tags
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Common;
    using Alexandria.Services.Mapping;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;

    public class TagsService : ITagsService
    {
        private readonly AlexandriaDbContext db;
        private readonly IMapper mapper;

        public TagsService(AlexandriaDbContext db)
        {
            this.db = db;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        public async Task CreateTagAsync(string name)
        {
            var tag = new Tag
            {
                Name = name,
                CreatedOn = DateTime.UtcNow,
            };

            await this.db.Tags.AddAsync(tag);
            await this.db.SaveChangesAsync();
        }

        public async Task DeleteTagByIdAsync(int id)
        {
            var tag = await this.GetByIdAsync(id);

            if (tag == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.TagNotFound, id));
            }

            tag.IsDeleted = true;
            tag.DeletedOn = DateTime.UtcNow;

            await this.db.SaveChangesAsync();
        }

        public async Task<TModel> GetTagByIdAsync<TModel>(int id)
        {
            var tag = await this.db.Tags.Where(t => t.Id == id && !t.IsDeleted)
                                  .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                                  .FirstOrDefaultAsync();

            return tag;
        }

        private async Task<Tag> GetByIdAsync(int id)
            => await this.db.Tags.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
    }
}
