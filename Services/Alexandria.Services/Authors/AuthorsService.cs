namespace Alexandria.Services.Authors
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class AuthorsService : IAuthorsService
    {
        private readonly AlexandriaDbContext db;

        public AuthorsService(AlexandriaDbContext db)
        {
            this.db = db;
        }

        public async Task<TModel> GetAuthorByIdAsync<TModel>(int id)
        {
            var author = await this.db.Authors.Where(a => a.Id == id && !a.IsDeleted)
                                              .To<TModel>()
                                              .FirstOrDefaultAsync();

            return author;
        }

        public async Task<bool> DoesAuthorIdExistAsync(int id)
            => await this.db.Authors.AnyAsync(a => a.Id == id && !a.IsDeleted);

        public async Task<IEnumerable<TModel>> GetAllAuthorsAsync<TModel>()
        {
            var authors = await this.db.Authors.AsNoTracking()
                                         .Where(a => !a.IsDeleted)
                                         .OrderBy(a => a.FirstName)
                                         .ThenBy(a => a.LastName)
                                         .To<TModel>()
                                         .ToListAsync();

            return authors;
        }
    }
}
