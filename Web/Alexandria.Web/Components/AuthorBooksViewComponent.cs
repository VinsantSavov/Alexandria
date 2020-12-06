namespace Alexandria.Web.Components
{
    using System.Threading.Tasks;

    using Alexandria.Services.Authors;
    using Alexandria.Services.Books;
    using Alexandria.Web.ViewModels.Books;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent(Name = "AuthorBooks")]
    public class AuthorBooksViewComponent : ViewComponent
    {
        private const int Count = 3;

        private readonly IBooksService booksService;
        private readonly IAuthorsService authorsService;

        public AuthorBooksViewComponent(
            IBooksService booksService,
            IAuthorsService authorsService)
        {
            this.booksService = booksService;
            this.authorsService = authorsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int authorId)
        {
            var viewModel = await this.authorsService.GetAuthorByIdAsync<BooksAuthorBooksViewModel>(authorId);
            viewModel.TopBooks = await this.booksService.GetTopRatedBooksByAuthorIdAsync<BooksSingleAuthorBookViewModel>(authorId, Count);

            return this.View(viewModel);
        }
    }
}
