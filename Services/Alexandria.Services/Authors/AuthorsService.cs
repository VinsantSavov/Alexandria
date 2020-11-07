namespace Alexandria.Services.Authors
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

    public class AuthorsService : IAuthorsService
    {
        private readonly AlexandriaDbContext db;
        private readonly IMapper mapper;

        public AuthorsService(AlexandriaDbContext db)
        {
            this.db = db;
            this.mapper = AutoMapperConfig.MapperInstance;
        }

        public async Task CreateAuthorAsync(string firstName, string profilePicture, string lastName, int countryId, string biography, DateTime bornOn)
        {
            var author = new Author
            {
                FirstName = firstName,
                LastName = lastName,
                ProfilePicture = profilePicture,
                CountryId = countryId,
                Biography = biography,
                BornOn = bornOn,
            };

            await this.db.Authors.AddAsync(author);
            await this.db.SaveChangesAsync();
        }

        public async Task<TModel> GetAuthorByIdAsync<TModel>(int id)
        {
            var author = await this.db.Authors.Where(a => a.Id == id && !a.IsDeleted)
                                            .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                                            .FirstOrDefaultAsync();

            return author;
        }

        public async Task DeleteAuthorByIdAsync(int id)
        {
            var author = await this.db.Authors.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

            if (author == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.AuthorNotFound, id));
            }

            author.IsDeleted = true;
            author.DeletedOn = DateTime.UtcNow;

            await this.db.SaveChangesAsync();
        }

        public async Task EditAuthorBiographyAsync(int id, string biography)
        {
            var author = await this.db.Authors.FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);

            if (author == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.AuthorNotFound, id));
            }

            author.Biography = biography;
            author.ModifiedOn = DateTime.UtcNow;

            await this.db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllAuthorsAsync<TModel>()
        {
            var authors = await this.db.Authors.AsNoTracking()
                                         .Where(a => !a.IsDeleted)
                                         .OrderBy(a => a.FirstName)
                                         .ThenBy(a => a.LastName)
                                         .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                                         .ToListAsync();

            return authors;
        }

        public async Task<IEnumerable<TModel>> GetMostPopularAuthorsByCountryAsync<TModel>(int countryId, int count = 0)
        {
            var authors = await this.db.Authors.AsNoTracking()
                                         .Where(a => a.CountryId == countryId && !a.IsDeleted)
                                         .OrderByDescending(a => a.Books.Average(b => b.Ratings
                                                                        .Where(r => !r.IsDeleted)
                                                                        .Average(r => r.Rate)))
                                         .ThenBy(a => a.FirstName)
                                         .ThenBy(a => a.LastName)
                                         .Take(count)
                                         .ProjectTo<TModel>(this.mapper.ConfigurationProvider)
                                         .ToListAsync();

            return authors;
        }
    }
}
