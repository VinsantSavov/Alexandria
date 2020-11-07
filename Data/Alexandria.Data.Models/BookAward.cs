namespace Alexandria.Data.Models
{
    public class BookAward
    {
        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        public int AwardId { get; set; }

        public virtual Award Award { get; set; }
    }
}
