namespace Alexandria.Services.Tags
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Common;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class TagsService : ITagsService
    {
        private readonly AlexandriaDbContext db;

        public TagsService(AlexandriaDbContext db)
        {
            this.db = db;
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

        public async Task<bool> DoesTagIdExistAsync(int id)
            => await this.db.Tags.AnyAsync(t => t.Id == id && !t.IsDeleted);

        public async Task<IEnumerable<TModel>> GetAllTagsAsync<TModel>()
        {
            return await this.db.Tags.AsNoTracking()
                                     .Where(t => !t.IsDeleted)
                                     .OrderBy(t => t.Name)
                                     .To<TModel>()
                                     .ToListAsync();
        }

        public async Task<TModel> GetTagByIdAsync<TModel>(int id)
        {
            var tag = await this.db.Tags.Where(t => t.Id == id && !t.IsDeleted)
                                  .To<TModel>()
                                  .FirstOrDefaultAsync();

            return tag;
        }

        private async Task<Tag> GetByIdAsync(int id)
            => await this.db.Tags.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
    }
}
