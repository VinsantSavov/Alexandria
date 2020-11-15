namespace Alexandria.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Alexandria.Data.Common.Models;

    public class Author : IAuditInfo, IDeletableEntity
    {
        public Author()
        {
            this.Books = new HashSet<Book>();
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string LastName { get; set; }

        public string ProfilePicture { get; set; }

        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public string Biography { get; set; }

        public DateTime DateOfBirth { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
