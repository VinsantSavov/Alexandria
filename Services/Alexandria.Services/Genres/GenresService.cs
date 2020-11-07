namespace Alexandria.Services.Genres
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

    public class GenresService : IGenresService
    {
        private readonly AlexandriaDbContext db;
        private readonly IMapper mapper;

        public GenresService(AlexandriaDbContext db)
        {
            this.db = db;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        public async Task CreateGenreAsync(string name, string description)
        {
            var genre = new Genre
            {
                Name = name,
                Description = description,
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
                                      .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                                      .FirstOrDefaultAsync();

            return genre;
        }

        private async Task<Genre> GetByIdAsync(int id)
            => await this.db.Genres.FirstOrDefaultAsync(g => g.Id == id && !g.IsDeleted);
    }
}
