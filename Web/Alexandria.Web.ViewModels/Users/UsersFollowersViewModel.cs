namespace Alexandria.Web.ViewModels.Users
{
    using System.Collections.Generic;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class UsersFollowersViewModel : PagingViewModel, IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }

        [IgnoreMap]
        public IEnumerable<UsersSingleFollowingViewModel> Following { get; set; }

        [IgnoreMap]
        public IEnumerable<UsersSingleFollowerViewModel> Followers { get; set; }

        public override string GetId()
        {
            return this.Id;
        }
    }
}
