namespace Alexandria.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Alexandria.Data.Common.Models;
    using Alexandria.Data.Models.Enums;

    public class Review : IAuditInfo, IDeletableEntity
    {
        public Review()
        {
            this.Likes = new HashSet<Like>();
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        public bool IsBestReview { get; set; }

        public int? ParentId { get; set; }

        public virtual Review Parent { get; set; }

        public string AuthorId { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        public ReadingProgress ReadingProgress { get; set; }

        public bool ThisEdition { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Like> Likes { get; set; }
    }
}
