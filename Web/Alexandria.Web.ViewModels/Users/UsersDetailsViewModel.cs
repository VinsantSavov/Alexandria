namespace Alexandria.Web.ViewModels.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Alexandria.Data.Models;
    using Alexandria.Data.Models.Enums;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class UsersDetailsViewModel : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public string Id { get; set; }

        public string Username { get; set; }

        public GenderType Gender { get; set; }

        public string ProfilePicture { get; set; }

        public string Biography { get; set; }

        public DateTime CreatedOn { get; set; }

        public int ReviewsCount { get; set; }

        public int RatingsCount { get; set; }

        public int FollowersCount { get; set; }

        public int FollowingCount { get; set; }

        public bool UserFollowedUser { get; set; }

        public IEnumerable<UsersSingleRatingViewModel> TopRatings { get; set; }

        public IEnumerable<UsersSingleReviewViewModel> TopReviews { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ApplicationUser, UsersDetailsViewModel>()
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
