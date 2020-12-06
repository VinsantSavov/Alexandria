namespace Alexandria.Web.Components
{
    using System.Threading.Tasks;

    using Alexandria.Services.Books;
    using Alexandria.Web.ViewModels.Books;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent(Name = "RandomBooks")]
    public class RandomBooksViewComponent : ViewComponent
    {
        private const int Count = 8;

        private readonly IBooksService booksService;

        public RandomBooksViewComponent(IBooksService booksService)
        {
            this.booksService = booksService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var books = await this.booksService.GetRandomBooksAsync<BooksRandomViewModel>(Count);

            return this.View(books);
        }
    }
}
