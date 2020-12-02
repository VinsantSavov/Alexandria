namespace Alexandria.Web.ViewModels.Users
{
    using System;
    using System.Collections.Generic;

    using Alexandria.Data.Models;
    using Alexandria.Data.Models.Enums;
    using Alexandria.Services.Mapping;

    public class UsersDetailsViewModel : IMapFrom<ApplicationUser>
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

        public IEnumerable<UsersSingleRatingViewModel> TopRatings { get; set; }

        public IEnumerable<UsersSingleReviewViewModel> TopReviews { get; set; }
    }
}
