namespace Alexandria.Services.Books
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Common;
    using Alexandria.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class BooksService : IBooksService
    {
        private readonly AlexandriaDbContext db;

        public BooksService(AlexandriaDbContext db)
        {
            this.db = db;
        }

        public async Task<int> CreateBookAsync(string title, int authorId, string summary, DateTime publishedOn, int pages, string pictureUrl, int editionLanguageId, string amazonLink, IEnumerable<int> genresIds, IEnumerable<int> tagsIds, IEnumerable<int> awardsIds)
        {
            var book = new Book
            {
                Title = title,
                AuthorId = authorId,
                Summary = summary,
                PublishedOn = publishedOn,
                Pages = pages,
                PictureURL = pictureUrl,
                EditionLanguageId = editionLanguageId,
                AmazonLink = amazonLink,
                CreatedOn = DateTime.UtcNow,
            };

            await this.db.Books.AddAsync(book);

            foreach (var id in genresIds)
            {
                book.Genres.Add(new BookGenre
                {
                    BookId = book.Id,
                    GenreId = id,
                });
            }

            foreach (var id in tagsIds)
            {
                book.Tags.Add(new BookTag
                {
                    BookId = book.Id,
                    TagId = id,
                });
            }

            foreach (var id in awardsIds)
            {
                book.Awards.Add(new BookAward
                {
                    BookId = book.Id,
                    AwardId = id,
                });
            }

            await this.db.SaveChangesAsync();

            return book.Id;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var book = await this.GetByIdAsync(id);

            if (book == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.BookNotFound, id));
            }

            book.IsDeleted = true;
            book.DeletedOn = DateTime.UtcNow;

            await this.db.SaveChangesAsync();
        }

        public async Task EditBookAsync(int id, string title, string summary, DateTime publishedOn, int pages, string pictureUrl, string amazonLink)
        {
            var book = await this.GetByIdAsync(id);

            book.Title = title;
            book.Summary = summary;
            book.PublishedOn = publishedOn;
            book.Pages = pages;
            book.PictureURL = pictureUrl;
            book.AmazonLink = amazonLink;
            book.ModifiedOn = DateTime.UtcNow;

            await this.db.SaveChangesAsync();
        }

        public async Task<string> GetPictureUrlByBookIdAsync(int id)
            => await this.db.Books.Where(b => b.Id == id && !b.IsDeleted).Select(b => b.PictureURL).FirstOrDefaultAsync();

        public async Task<TModel> GetBookByIdAsync<TModel>(int id)
        {
            var book = await this.db.Books.Where(b => b.Id == id && !b.IsDeleted)
                                    .To<TModel>()
                                    .FirstOrDefaultAsync();

            if (book == null)
            {
                throw new NullReferenceException(string.Format(ExceptionMessages.BookNotFound, id));
            }

            return book;
        }

        public async Task<IEnumerable<TModel>> SearchBooksByTitleAsync<TModel>(string search = null, int? take = null, int skip = 0)
        {
            var books = this.db.Books.Where(b => !b.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search) && take.HasValue)
            {
                books = books.Where(b => b.Title.ToLower().Contains(search.ToLower()))
                             .OrderBy(b => b.Title)
                             .Skip(skip)
                             .Take(take.Value);
            }

            return await books.To<TModel>().ToListAsync();
        }

        public async Task<IEnumerable<TModel>> GetRandomBooksAsync<TModel>(int count)
        {
            return await this.db.Books.Where(b => !b.IsDeleted)
                                      .OrderBy(b => Guid.NewGuid())
                                      .To<TModel>()
                                      .Take(count)
                                      .ToListAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllBooksByGenreIdAsync<TModel>(int genreId)
        {
            var books = await this.db.Books.AsNoTracking()
                                     .Where(b => b.Genres.Any(g => g.GenreId == genreId) && !b.IsDeleted)
                                     .OrderByDescending(b => b.Ratings
                                                              .Average(r => r.Rate))
                                     .ThenBy(b => b.Title)
                                     .To<TModel>()
                                     .ToListAsync();

            return books;
        }

        public async Task<bool> DoesBookIdExistAsync(int id)
        {
            return await this.db.Books.AnyAsync(b => b.Id == id && !b.IsDeleted);
        }

        public async Task<int> GetBooksCountAsync(string search = null)
        {
            var books = this.db.Books.Where(b => !b.IsDeleted);

            if (search != null)
            {
                return await books.Where(b => b.Title.ToLower().Contains(search.ToLower())).CountAsync();
            }

            return await books.CountAsync();
        }

        public async Task<int> GetBooksCountByAuthorIdAsync(int authorId)
        {
            int count = await this.db.Books.Where(b => b.AuthorId == authorId && !b.IsDeleted)
                                           .CountAsync();

            return count;
        }

        public async Task<int> GetBooksCountByGenreIdAsync(int genreId)
        {
            int count = await this.db.Books.Where(b => b.Genres.Any(g => g.GenreId == genreId) && !b.IsDeleted)
                                           .CountAsync();

            return count;
        }

        public async Task<IEnumerable<TModel>> NewRealesedBooksByGenreIdAsync<TModel>(int genreId, int? take = null, int skip = 0)
        {
            // thenBy rating?
            var queryable = this.db.Books.AsNoTracking()
                                         .Where(b => b.Genres
                                                      .Any(g => g.GenreId == genreId) && !b.IsDeleted)
                                         .OrderByDescending(b => b.PublishedOn)
                                         .Skip(skip);

            if (take.HasValue)
            {
                queryable = queryable.Take(take.Value);
            }

            return await queryable.To<TModel>().ToListAsync();
        }

        public async Task<IEnumerable<TModel>> TopRatedBooksByGenreIdAsync<TModel>(int genreId, int? take = null, int skip = 0)
        {
            var queryable = this.db.Books.AsNoTracking()
                                     .Where(b => b.Genres.Any(g => g.GenreId == genreId) && !b.IsDeleted)
                                     .OrderByDescending(b => b.Ratings
                                                              .Average(r => r.Rate))
                                     .Skip(skip);

            if (take.HasValue)
            {
                queryable = queryable.Take(take.Value);
            }

            return await queryable.To<TModel>().ToListAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllBooksByTagIdAsync<TModel>(int tagId)
        {
            var books = await this.db.Books.AsNoTracking()
                                     .Where(b => b.Tags.Any(t => t.TagId == tagId) && !b.IsDeleted)
                                     .OrderByDescending(b => b.Ratings
                                                              .Average(r => r.Rate))
                                     .To<TModel>()
                                     .ToListAsync();

            return books;
        }

        public async Task<IEnumerable<TModel>> GetLatestPublishedBooksAsync<TModel>(int? take = null, int skip = 0)
        {
            var queryable = this.db.Books.AsNoTracking()
                                     .Where(b => !b.IsDeleted)
                                     .OrderByDescending(b => b.PublishedOn)
                                     .ThenByDescending(b => b.Ratings
                                                             .Average(r => r.Rate))
                                     .ThenBy(b => b.Title)
                                     .Skip(skip);

            if (take.HasValue)
            {
                queryable = queryable.Take(take.Value);
            }

            return await queryable.To<TModel>().ToListAsync();
        }

        public async Task<IEnumerable<TModel>> GetAllBooksByAuthorIdAsync<TModel>(int authorId, int? take = null, int skip = 0)
        {
            var queryable = this.db.Books.AsNoTracking()
                                     .Where(b => b.AuthorId == authorId && !b.IsDeleted)
                                     .OrderByDescending(b => b.Ratings
                                                              .Average(r => r.Rate))
                                     .ThenByDescending(b => b.PublishedOn)
                                     .ThenBy(b => b.Title)
                                     .Skip(skip);

            if (take.HasValue)
            {
                queryable = queryable.Take(take.Value);
            }

            return await queryable.To<TModel>().ToListAsync();
        }

        public async Task<IEnumerable<TModel>> GetTopRatedBooksByAuthorIdAsync<TModel>(int authorId, int count = 0)
        {
            var books = await this.db.Books.AsNoTracking()
                                     .Where(b => b.AuthorId == authorId && !b.IsDeleted)
                                     .OrderByDescending(b => b.Ratings
                                                              .Average(r => r.Rate))
                                     .To<TModel>()
                                     .Take(count)
                                     .ToListAsync();

            return books;
        }

        public async Task<IEnumerable<TModel>> GetBooksWithMostAwardsAsync<TModel>(int count = 0)
        {
            var books = await this.db.Books.AsNoTracking()
                                     .Where(b => !b.IsDeleted)
                                     .OrderByDescending(b => b.Awards.Count)
                                     .ThenBy(b => b.Title)
                                     .Take(count)
                                     .To<TModel>()
                                     .ToListAsync();

            return books;
        }

        public async Task<IEnumerable<TModel>> GetBooksWithMostReviewsAsync<TModel>(int count = 0)
        {
            var books = await this.db.Books.AsNoTracking()
                                     .Where(b => !b.IsDeleted)
                                     .OrderByDescending(b => b.Reviews.Count)
                                     .ThenBy(b => b.Title)
                                     .Take(count)
                                     .To<TModel>()
                                     .ToListAsync();

            return books;
        }

        public async Task<IEnumerable<TModel>> GetAllBooksAsync<TModel>(int? take = null, int skip = 0)
        {
            var queryable = this.db.Books.AsNoTracking()
                                         .Where(b => !b.IsDeleted)
                                         .OrderBy(b => b.Title)
                                         .Skip(skip);

            if (take.HasValue)
            {
                queryable = queryable.Take(take.Value);
            }

            return await queryable.To<TModel>().ToListAsync();
        }

        public async Task<IEnumerable<TModel>> GetTopRatedBooksAsync<TModel>(int? take = null, int skip = 0)
        {
            var queryable = this.db.Books.AsNoTracking()
                                     .Where(b => !b.IsDeleted)
                                     .OrderByDescending(b => b.Ratings.Average(r => r.Rate))
                                     .Skip(skip);

            if (take.HasValue)
            {
                queryable = queryable.Take(take.Value);
            }

            return await queryable.To<TModel>().ToListAsync();
        }

        private async Task<Book> GetByIdAsync(int id)
            => await this.db.Books.FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);
    }
}
