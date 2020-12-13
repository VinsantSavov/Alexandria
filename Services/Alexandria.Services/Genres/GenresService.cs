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

        public async Task<int> CreateGenreAsync(string name, string description)
        {
            var genre = new Genre
            {
                Name = name,
                Description = description,
                CreatedOn = DateTime.UtcNow,
            };

            await this.db.Genres.AddAsync(genre);
            await this.db.SaveChangesAsync();

            return genre.Id;
        }

        public async Task EditGenreAsync(int id, string name, string description)
        {
            var genre = await this.GetByIdAsync(id);

            genre.Name = name;
            genre.Description = description;

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

        public async Task<bool> DoesGenreIdExistAsync(int id)
            => await this.db.Genres.AnyAsync(g => g.Id == id && !g.IsDeleted);

        public async Task<bool> DoesGenreNameExistAsync(string name)
            => await this.db.Genres.AnyAsync(g => g.Name == name && !g.IsDeleted);

        public async Task<IEnumerable<TModel>> GetAllGenresAsync<TModel>(int? take = null, int skip = 0)
        {
            var queryable = this.db.Genres.AsNoTracking()
                                          .Where(g => !g.IsDeleted)
                                          .OrderBy(g => g.Name)
                                          .Skip(skip);

            if (take.HasValue)
            {
                queryable = queryable.Take(take.Value);
            }

            return await queryable.To<TModel>().ToListAsync();
        }

        public async Task<int> GetGenresCount()
         => await this.db.Genres.Where(g => !g.IsDeleted).CountAsync();

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
