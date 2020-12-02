namespace Alexandria.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class UsersReviewViewModel : PagingViewModel, IMapFrom<ApplicationUser>
    {
        public new string Id { get; set; }

        public string Username { get; set; }

        public IEnumerable<UsersSingleReviewViewModel> AllReviews { get; set; }
    }
}
