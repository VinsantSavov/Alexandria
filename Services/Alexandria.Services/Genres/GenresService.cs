namespace Alexandria.Services.Genres
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

    public class GenresService : IGenresService
    {
        private readonly AlexandriaDbContext db;

        public GenresService(AlexandriaDbContext db)
        {
            this.db = db;
        }

        public async Task CreateGenreAsync(string name, string description)
        {
            var genre = new Genre
            {
                Name = name,
                Description = description,
                CreatedOn = DateTime.UtcNow,
            };

            await this.db.Genres.AddAsync(genre);
            await this.db.SaveChangesAsync();
        }

        public async Task DeleteGenreByIdAsync(int id)
        {
            var genre = await this.GetByIdAsync(id);

            if (genre == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.GenreNotFound, id));
            }

            genre.IsDeleted = true;
            genre.DeletedOn = DateTime.UtcNow;

            await this.db.SaveChangesAsync();
        }

        public async Task<TModel> GetGenreByIdAsync<TModel>(int id)
        {
            var genre = await this.db.Genres.Where(g => g.Id == id && !g.IsDeleted)
                                      .To<TModel>()
                                      .FirstOrDefaultAsync();

            return genre;
        }

        public async Task<IEnumerable<TModel>> GetRandomGenresAsync<TModel>(int count)
        {
            return await this.db.Genres.AsNoTracking()
                                       .Where(g => !g.IsDeleted)
                                       .OrderBy(g => Guid.NewGuid())
                                       .Take(count)
                                       .To<TModel>()
                                       .ToListAsync();
        }

        private async Task<Genre> GetByIdAsync(int id)
            => await this.db.Genres.FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted);
    }
}
