namespace Alexandria.Web.ViewModels.Users
{
    using System.Linq;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class UsersSingleFollowerViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public string ProfilePicture { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }

        public int ReviewsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UsersSingleFollowerViewModel>()
                .ForMember(
                           u => u.ReviewsCount,
                           au => au.MapFrom(src => src.Reviews
                           .Where(r => !r.IsDeleted)
                           .Count()))
                .ForMember(
                           u => u.FollowersCount,
                           au => au.MapFrom(src => src.Followers
                           .Where(f => !f.IsDeleted)
                           .Count()))
                .ForMember(
                           u => u.FollowingCount,
                           au => au.MapFrom(src => src.Following
                           .Where(f => !f.IsDeleted)
                           .Count()));
        }
    }
}
