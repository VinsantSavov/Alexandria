namespace Alexandria.Services.BookTags
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBookTagsService
    {
        Task AddTagsToBookAsync(int bookId, IEnumerable<int> tagsIds);
    }
}
