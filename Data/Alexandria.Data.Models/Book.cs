namespace Alexandria.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Alexandria.Data.Common.Models;

    public class Book : IAuditInfo, IDeletableEntity
    {
        public Book()
        {
            this.Awards = new HashSet<BookAward>();
            this.Genres = new HashSet<BookGenre>();
            this.Tags = new HashSet<BookTag>();
            this.Reviews = new HashSet<Review>();
            this.Ratings = new HashSet<StarRating>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int AuthorId { get; set; }

        public virtual Author Author { get; set; }

        public string Summary { get; set; }

        public DateTime PublishedOn { get; set; }

        public int Pages { get; set; }

        public string PictureURL { get; set; }

        public int EditionLanguageId { get; set; }

        public virtual EditionLanguage EditionLanguage { get; set; }

        public string AmazonLink { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<BookAward> Awards { get; set; }

        public virtual ICollection<BookGenre> Genres { get; set; }

        public virtual ICollection<BookTag> Tags { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<StarRating> Ratings { get; set; }
    }
}
