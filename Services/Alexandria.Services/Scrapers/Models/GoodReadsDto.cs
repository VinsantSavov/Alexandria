namespace Alexandria.Services.Scrapers.Models
{
    using System;
    using System.Collections.Generic;

    public class GoodReadsDto
    {
        public GoodReadsDto()
        {
            this.Awards = new List<string>();
            this.Genres = new List<string>();
        }

        public string Title { get; set; }

        public string AuthorFirstName { get; set; }

        public string AuthorSecondName { get; set; }

        public string AuthorLastName { get; set; }

        public DateTime AuthorDateOfBirth { get; set; }

        public string AuthorCountry { get; set; }

        public string AuthorBiography { get; set; }

        public string AuthorPicture { get; set; }

        public string Summary { get; set; }

        public string Image { get; set; }

        public int Pages { get; set; }

        public DateTime PublishedOn { get; set; }

        public string EditionLanguage { get; set; }

        public ICollection<string> Awards { get; set; }

        public ICollection<string> Genres { get; set; }
    }
}
