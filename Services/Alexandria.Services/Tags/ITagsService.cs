namespace Alexandria.Services.Tags
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITagsService
    {
        Task CreateTagAsync(string name);

        Task DeleteTagByIdAsync(int id);

        Task<TModel> GetTagByIdAsync<TModel>(int id);

        Task<IEnumerable<TModel>> GetAllTagsAsync<TModel>();
    }
}
