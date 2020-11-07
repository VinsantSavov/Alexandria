namespace Alexandria.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Alexandria.Data.Common.Models;

    public class Genre : IDeletableEntity
    {
        public Genre()
        {
            this.Books = new HashSet<BookGenre>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<BookGenre> Books { get; set; }
    }
}
