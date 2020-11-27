namespace Alexandria.Data.Models
{
    using System;

    using Alexandria.Data.Common.Models;

    public class Like : IAuditInfo
    {
        public int Id { get; set; }

        public bool IsLiked { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public int ReviewId { get; set; }

        public Review Review { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
