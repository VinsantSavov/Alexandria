namespace Alexandria.Services.BookTags
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;

    public class BookTagsService : IBookTagsService
    {
        private readonly AlexandriaDbContext db;

        public BookTagsService(AlexandriaDbContext db)
        {
            this.db = db;
        }

        public async Task AddTagsToBook(int bookId, IEnumerable<int> tagsIds)
        {
            foreach (var id in tagsIds)
            {
                await this.db.BookTags.AddAsync(new BookTag
                {
                    BookId = bookId,
                    TagId = id,
                });
            }

            await this.db.SaveChangesAsync();
        }
    }
}
