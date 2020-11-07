namespace Alexandria.Data.Models
{
    using System;

    using Alexandria.Data.Common.Models;

    public class StarRating : IDeletableEntity
    {
        public int Id { get; set; }

        public int Rate { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
    }
}
