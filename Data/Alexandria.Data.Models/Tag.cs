namespace Alexandria.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Alexandria.Data.Common.Models;

    public class Tag : IAuditInfo, IDeletableEntity
    {
        public Tag()
        {
            this.Books = new HashSet<BookTag>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<BookTag> Books { get; set; }
    }
}
