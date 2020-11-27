namespace Alexandria.Services.Likes
{
    using System.Threading.Tasks;

    public interface ILikesService
    {
        Task CreateLikeAsync(string userId, int reviewId, bool isLiked);

        Task<int> GetLikesCountByReviewIdAsync(int reviewId);
    }
}
