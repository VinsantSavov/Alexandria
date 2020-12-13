namespace Alexandria.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Alexandria.Services.Authors;
    using Alexandria.Services.Awards;
    using Alexandria.Services.Books;
    using Alexandria.Services.BookTags;
    using Alexandria.Services.Cloudinary;
    using Alexandria.Services.EditionLanguages;
    using Alexandria.Services.Genres;
    using Alexandria.Services.Tags;
    using Alexandria.Web.ViewModels.Administration.Books;
    using Microsoft.AspNetCore.Mvc;

    public class BooksController : AdministrationController
    {
        public const int BooksPerPage = 10;
        public const string ControllerName = "Books";

        private readonly IBooksService booksService;
        private readonly IGenresService genresService;
        private readonly ITagsService tagsService;
        private readonly IAwardsService awardsService;
        private readonly IAuthorsService authorsService;
        private readonly IEditionLanguagesService languagesService;
        private readonly IBookTagsService bookTagsService;
        private readonly ICloudinaryService cloudinaryService;

        public BooksController(
            IBooksService booksService,
            IGenresService genresService,
            ITagsService tagsService,
            IAwardsService awardsService,
            IAuthorsService authorsService,
            IEditionLanguagesService languagesService,
            IBookTagsService bookTagsService,
            ICloudinaryService cloudinaryService)
        {
            this.booksService = booksService;
            this.genresService = genresService;
            this.tagsService = tagsService;
            this.awardsService = awardsService;
            this.authorsService = authorsService;
            this.languagesService = languagesService;
            this.bookTagsService = bookTagsService;
            this.cloudinaryService = cloudinaryService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var viewModel = new ABooksAllBooksViewModel();

            var booksCount = await this.booksService.GetBooksCountAsync();

            viewModel.Books = await this.booksService.GetAllBooksAsync<ABooksListingViewModel>(BooksPerPage, (page - 1) * BooksPerPage);
            viewModel.CurrentPage = page;
            viewModel.PagesCount = (int)Math.Ceiling((double)booksCount / BooksPerPage);
            viewModel.ControllerName = ControllerName;
            viewModel.ActionName = nameof(this.Index);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var book = await this.booksService.GetBookByIdAsync<ABooksDetailsViewModel>(id.Value);

            if (book == null)
            {
                return this.NotFound();
            }

            return this.View(book);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new ABooksCreateInputModel();
            viewModel.Genres = await this.genresService.GetAllGenresAsync<ABooksGenreViewModel>();
            viewModel.Tags = await this.tagsService.GetAllTagsAsync<ABooksTagViewModel>();
            viewModel.Awards = await this.awardsService.GetAllAwardsAsync<ABooksAwardViewModel>();
            viewModel.Authors = await this.authorsService.GetAllAuthorsAsync<ABooksAuthorViewModel>();
            viewModel.Languages = await this.languagesService.GetAllLanguagesAsync<ABooksEditionLanguageViewModel>();

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ABooksCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.Genres = await this.genresService.GetAllGenresAsync<ABooksGenreViewModel>();
                input.Tags = await this.tagsService.GetAllTagsAsync<ABooksTagViewModel>();
                input.Awards = await this.awardsService.GetAllAwardsAsync<ABooksAwardViewModel>();
                input.Authors = await this.authorsService.GetAllAuthorsAsync<ABooksAuthorViewModel>();
                input.Languages = await this.languagesService.GetAllLanguagesAsync<ABooksEditionLanguageViewModel>();

                return this.View(input);
            }

            var bookCoverUrl = await this.cloudinaryService.UploadImageAsync(input.Cover, input.Title);

            var bookId = await this.booksService.CreateBookAsync(input.Title, input.AuthorId, input.Summary, input.PublishedOn, input.Pages, bookCoverUrl, input.EditionLanguageId, input.AmazonLink, input.GenresIds, input.TagsIds, input.AwardsIds);

            return this.RedirectToAction(nameof(this.Details), new { Id = bookId });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var book = await this.booksService.GetBookByIdAsync<ABooksEditInputModel>(id.Value);

            if (book == null)
            {
                return this.NotFound();
            }

            return this.View(book);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ABooksEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input = await this.booksService.GetBookByIdAsync<ABooksEditInputModel>(input.Id);
                return this.View(input);
            }

            if (input.Cover != null)
            {
                input.PictureURL = await this.cloudinaryService.UploadImageAsync(input.Cover, input.Title);
            }
            else
            {
                input.PictureURL = await this.booksService.GetPictureUrlByBookIdAsync(input.Id);
            }

            await this.booksService.EditBookAsync(input.Id, input.Title, input.Summary, input.PublishedOn, input.Pages, input.PictureURL, input.AmazonLink);

            return this.RedirectToAction(nameof(this.Details), new { id = input.Id });
        }

        public async Task<IActionResult> AddTags(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var viewModel = await this.booksService.GetBookByIdAsync<ABooksAddTagsInputModel>(id.Value);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            viewModel.AllTags = await this.tagsService.GetAllTagsAsync<ABooksTagViewModel>();

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddTags(ABooksAddTagsInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input = await this.booksService.GetBookByIdAsync<ABooksAddTagsInputModel>(input.Id);
                input.AllTags = await this.tagsService.GetAllTagsAsync<ABooksTagViewModel>();

                return this.View(input);
            }

            await this.bookTagsService.AddTagsToBookAsync(input.Id, input.TagsIds);

            return this.RedirectToAction(nameof(this.Details), new { Id = input.Id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var book = await this.booksService.GetBookByIdAsync<ABooksDeleteViewModel>(id.Value);

            if (book == null)
            {
                return this.NotFound();
            }

            return this.View(book);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doExist = await this.booksService.DoesBookIdExistAsync(id);

            if (!doExist)
            {
                return this.NotFound();
            }

            await this.booksService.DeleteByIdAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
