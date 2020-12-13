namespace Alexandria.Services.BookTags
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class BookTagsService : IBookTagsService
    {
        private readonly AlexandriaDbContext db;

        public BookTagsService(AlexandriaDbContext db)
        {
            this.db = db;
        }

        public async Task AddTagsToBookAsync(int bookId, IEnumerable<int> tagsIds)
        {
            foreach (var id in tagsIds)
            {
                if (!await this.db.BookTags.AnyAsync(bt => bt.BookId == bookId
                                                         && bt.TagId == id))
                {
                    var bookTag = new BookTag
                    {
                        BookId = bookId,
                        TagId = id,
                    };

                    await this.db.BookTags.AddAsync(bookTag);
                    await this.db.SaveChangesAsync();
                }
            }
        }
    }
}
