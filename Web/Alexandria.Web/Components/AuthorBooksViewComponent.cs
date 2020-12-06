namespace Alexandria.Web.Components
{
    using System.Threading.Tasks;

    using Alexandria.Services.Books;
    using Alexandria.Web.ViewModels.Books;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent(Name = "AuthorBooks")]
    public class AuthorBooksViewComponent : ViewComponent
    {
        private const int Count = 3;

        private readonly IBooksService booksService;

        public AuthorBooksViewComponent(IBooksService booksService)
        {
            this.booksService = booksService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int authorId)
        {
            var books = await this.booksService.GetTopRatedBooksByAuthorIdAsync<BooksAuthorBooksViewModel>(authorId, Count);

            return this.View(books);
        }
    }
}
