namespace Alexandria.Services.Books
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
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
                if (!await this.db.BookGenres.AnyAsync(bg => bg.BookId == book.Id
                                                        && bg.GenreId == id))
                {
                    var bookGenre = new BookGenre
                    {
                        BookId = book.Id,
                        GenreId = id,
                    };

                    book.Genres.Add(bookGenre);
                    await this.db.SaveChangesAsync();
                }
            }

            foreach (var id in tagsIds)
            {
                if (!await this.db.BookTags.AnyAsync(bt => bt.BookId == book.Id
                                                     && bt.TagId == id))
                {
                    var bookTag = new BookTag
                    {
                        BookId = book.Id,
                        TagId = id,
                    };

                    book.Tags.Add(bookTag);
                    await this.db.SaveChangesAsync();
                }
            }

            foreach (var id in awardsIds)
            {
                if (!await this.db.BookAwards.AnyAsync(ba => ba.BookId == book.Id
                                                       && ba.AwardId == id))
                {
                    var bookAward = new BookAward
                    {
                        BookId = book.Id,
                        AwardId = id,
                    };

                    book.Awards.Add(bookAward);
                    await this.db.SaveChangesAsync();
                }
            }

            await this.db.SaveChangesAsync();

            return book.Id;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var book = await this.GetByIdAsync(id);

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

            return book;
        }

        public async Task<IEnumerable<TModel>> SearchBooksByTitleAsync<TModel>(string search = null, int? take = null, int skip = 0)
        {
            var books = this.db.Books.Where(b => !b.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
            {
                books = books.Where(b => b.Title.ToLower().Contains(search.ToLower()))
                             .OrderBy(b => b.Title);
            }

            if (take.HasValue)
            {
                books = books.Skip(skip).Take(take.Value);
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

        public async Task<bool> DoesBookIdExistAsync(int id)
        {
            return await this.db.Books.AnyAsync(b => b.Id == id && !b.IsDeleted);
        }

        public async Task<int> GetBooksCountAsync(string search = null)
        {
            var books = this.db.Books.Where(b => !b.IsDeleted);

            if (!string.IsNullOrWhiteSpace(search))
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
