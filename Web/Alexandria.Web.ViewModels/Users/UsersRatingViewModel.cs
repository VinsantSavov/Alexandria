namespace Alexandria.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class UsersRatingViewModel : PagingViewModel, IMapFrom<ApplicationUser>
    {
        public new string Id { get; set; }

        public string Username { get; set; }

        public IEnumerable<UsersSingleRatingViewModel> AllRatings { get; set; }
    }
}
