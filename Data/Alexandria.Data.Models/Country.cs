namespace Alexandria.Data.Models
{
    using System.Collections.Generic;

    public class Country
    {
        public Country()
        {
            this.Authors = new HashSet<Author>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
    }
}
