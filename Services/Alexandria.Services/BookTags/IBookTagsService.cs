namespace Alexandria.Services.BookTags
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBookTagsService
    {
        Task AddTagsToBook(int bookId, IEnumerable<int> tagsIds);
    }
}
