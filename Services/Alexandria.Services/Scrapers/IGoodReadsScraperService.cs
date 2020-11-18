namespace Alexandria.Services.Scrapers
{
    using System.Threading.Tasks;

    public interface IGoodReadsScraperService
    {
        Task PopulateDatabaseWithBooks(int amount);
    }
}
