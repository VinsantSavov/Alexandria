// ReSharper disable VirtualMemberCallInConstructor
namespace Alexandria.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Alexandria.Data.Common.Models;
    using Alexandria.Data.Models.Enums;
    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();

            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.Reviews = new HashSet<Review>();
            this.Ratings = new HashSet<StarRating>();
            this.Likes = new HashSet<Like>();
            this.Followers = new HashSet<UserFollower>();
        }

        public GenderType Gender { get; set; }

        public string ProfilePicture { get; set; }

        public string Biography { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<StarRating> Ratings { get; set; }

        public virtual ICollection<Like> Likes { get; set; }

        public virtual ICollection<UserFollower> Followers { get; set; }
    }
}
