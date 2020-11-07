namespace Alexandria.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Alexandria.Data.Common.Models;

    public class Award : IDeletableEntity
    {
        public Award()
        {
            this.Books = new HashSet<BookAward>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<BookAward> Books { get; set; }
    }
}
