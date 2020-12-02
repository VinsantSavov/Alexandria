namespace Alexandria.Web.ViewModels.Users
{
    using System;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class UsersSingleRatingViewModel : IMapFrom<StarRating>
    {
        public int Id { get; set; }

        public int Rate { get; set; }

        public DateTime CreatedOn { get; set; }

        public UsersBookViewModel Book { get; set; }
    }
}
