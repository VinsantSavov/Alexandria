namespace Alexandria.Services.Scrapers
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Scrapers.Models;
    using AngleSharp;
    using Microsoft.EntityFrameworkCore;

    public class GoodReadsScraperService : IGoodReadsScraperService
    {
        private readonly IConfiguration config;
        private readonly IBrowsingContext context;
        private readonly AlexandriaDbContext db;

        public GoodReadsScraperService(AlexandriaDbContext db)
        {
            this.config = Configuration.Default.WithDefaultLoader();
            this.context = BrowsingContext.New(this.config);
            this.db = db;
        }

        public async Task PopulateDatabaseWithBooks(int amount)
        {
            var concurrentBag = new ConcurrentBag<GoodReadsDto>();

            for (int i = 1; i < amount; i++)
            {
                try
                {
                    var dto = this.GetBook(i);

                    if (dto == null)
                    {
                        continue;
                    }

                    concurrentBag.Add(dto);
                }
                catch
                {
                    Console.WriteLine($"Problem with book {i}");
                }
            }

            foreach (var bookModel in concurrentBag)
            {
                var country = await this.db.Countries.FirstOrDefaultAsync(c => bookModel.AuthorCountry.Contains(c.Name));
                if (country == null)
                {
                    continue;
                }

                var language = await this.db.EditionLanguages.FirstOrDefaultAsync(l => bookModel.EditionLanguage.Contains(l.Name));
                if (language == null)
                {
                    continue;
                }

                var author = new Author
                {
                    FirstName = bookModel.AuthorFirstName,
                    SecondName = bookModel.AuthorSecondName,
                    LastName = bookModel.AuthorLastName,
                    Biography = bookModel.AuthorBiography,
                    DateOfBirth = bookModel.AuthorDateOfBirth,
                    ProfilePicture = bookModel.AuthorPicture,
                    Country = country,
                    CreatedOn = DateTime.UtcNow,
                };
                await this.db.Authors.AddAsync(author);
                await this.db.SaveChangesAsync();

                var book = new Book
                {
                    Title = bookModel.Title,
                    Summary = bookModel.Summary,
                    PictureURL = bookModel.Image,
                    Pages = bookModel.Pages,
                    PublishedOn = bookModel.PublishedOn,
                    EditionLanguage = language,
                    Author = author,
                    CreatedOn = DateTime.UtcNow,
                };
                await this.db.Books.AddAsync(book);
                await this.db.SaveChangesAsync();

                foreach (var award in bookModel.Awards)
                {
                    var bookAward = await this.db.Awards.FirstOrDefaultAsync(a => a.Name == award);

                    if (bookAward == null)
                    {
                        bookAward = new Award
                        {
                            Name = award,
                            CreatedOn = DateTime.UtcNow,
                        };

                        await this.db.Awards.AddAsync(bookAward);
                        await this.db.SaveChangesAsync();
                    }

                    if (!this.db.BookAwards.Any(ba => ba.BookId == book.Id && ba.AwardId == bookAward.Id))
                    {
                        await this.db.BookAwards.AddAsync(new BookAward
                        {
                            Book = book,
                            Award = bookAward,
                        });
                        await this.db.SaveChangesAsync();
                    }
                }

                foreach (var genre in bookModel.Genres)
                {
                    var bookGenre = await this.db.Genres.FirstOrDefaultAsync(g => g.Name == genre);

                    if (bookGenre != null)
                    {
                        if (!this.db.BookGenres.Any(bg => bg.GenreId == bookGenre.Id && bg.BookId == book.Id))
                        {
                            await this.db.BookGenres.AddAsync(new BookGenre
                            {
                                Book = book,
                                Genre = bookGenre,
                            });
                            await this.db.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        private GoodReadsDto GetBook(int i)
        {
            var dto = new GoodReadsDto();

            var document = this.context.OpenAsync($"https://www.goodreads.com/book/show/{i}.Twilight")
                .GetAwaiter().GetResult();

            if (document.StatusCode == HttpStatusCode.NotFound)
            {
                throw new InvalidOperationException();
            }

            var title = document.QuerySelector("div > #metacol > #bookTitle");
            dto.Title = title.TextContent.Trim();

            var authorLink = document.QuerySelectorAll(".leftContainer > #topcol > #metacol > #bookAuthors > span > .authorName__container > .authorName").FirstOrDefault();

            var authorDocument = this.context.OpenAsync(authorLink.GetAttribute("href"))
                .GetAwaiter().GetResult();

            var authorName = authorDocument.QuerySelector(".rightContainer > div > .authorName > span");
            var names = authorName.TextContent.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            dto.AuthorFirstName = names[0];
            if (names.Length == 3)
            {
                dto.AuthorSecondName = names[1];
                dto.AuthorLastName = names[2];
            }
            else
            {
                dto.AuthorLastName = names[1];
            }

            var authorDateOfBirth = authorDocument.QuerySelector(".rightContainer > .dataItem");
            dto.AuthorDateOfBirth = DateTime.Parse(authorDateOfBirth.TextContent.Trim());

            var authorCountry = authorDocument.QuerySelector(".rightContainer");
            dto.AuthorCountry = authorCountry.TextContent;

            var authorBiography = authorDocument.QuerySelector(".rightContainer > .aboutAuthorInfo > span");
            dto.AuthorBiography = authorBiography.TextContent;

            // Can be null
            var authorPicture = authorDocument.QuerySelector(".leftContainer > a > img");
            var authPic = authorPicture.GetAttribute("src");
            dto.AuthorPicture = authPic;

            var summary = document.QuerySelector("#descriptionContainer > #description > span");
            dto.Summary = summary.TextContent;

            var image = document.QuerySelectorAll(".leftContainer > #topcol > #imagecol > .bookCoverContainer > .bookCoverPrimary > a > #coverImage").FirstOrDefault();
            dto.Image = image.GetAttribute("src");

            var pagesText = document.QuerySelectorAll("#details > div > span").LastOrDefault().TextContent.ToString();
            var pages = int.Parse(pagesText.Split(" ").FirstOrDefault());
            dto.Pages = pages;

            var publishedOn = document.QuerySelectorAll("#details > div").ToArray()[1].TextContent.ToString();
            var date = publishedOn.Remove(publishedOn.IndexOf("Published"), 9);
            date = date.Remove(date.IndexOf("by "), date.Length - date.IndexOf("by ")).Trim();
            if (date.Contains("th"))
            {
                date = date.Remove(date.IndexOf("th"), 2);
            }

            var realDate = DateTime.Parse(date);
            dto.PublishedOn = realDate;

            var editionLanguage = document.QuerySelectorAll("#details > .buttons > #bookDataBox > div > .infoBoxRowItem").ToArray()[2];
            dto.EditionLanguage = editionLanguage.TextContent;

            var awards = document.QuerySelectorAll("#details > .buttons > #bookDataBox > div > div > .award");
            if (awards != null)
            {
                foreach (var award in awards)
                {
                    dto.Awards.Add(award.TextContent);
                }
            }

            var genres = document.QuerySelectorAll(".rightContainer > .stacked > div > .bigBoxBody > .bigBoxContent > div > .left > a").ToArray();
            foreach (var genre in genres)
            {
                dto.Genres.Add(genre.TextContent);
            }

            Console.WriteLine("found");
            return dto;
        }
    }
}
