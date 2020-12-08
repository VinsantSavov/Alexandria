namespace Alexandria.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Awards;
    using Alexandria.Services.Books;
    using Alexandria.Services.Genres;
    using Alexandria.Services.Tags;
    using Alexandria.Web.ViewModels.Administration.Books;
    using Alexandria.Web.ViewModels.Books;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class BooksController : AdministrationController
    {
        public const int BooksPerPage = 10;
        public const string ControllerName = "Books";

        private readonly IBooksService booksService;
        private readonly IGenresService genresService;
        private readonly ITagsService tagsService;
        private readonly IAwardsService awardsService;

        public BooksController(
            IBooksService booksService,
            IGenresService genresService,
            ITagsService tagsService,
            IAwardsService awardsService)
        {
            this.booksService = booksService;
            this.genresService = genresService;
            this.tagsService = tagsService;
            this.awardsService = awardsService;
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

                return this.View(input);
            }

            return this.RedirectToAction(nameof(this.Details), new { Id = 0 });
        }

        /*// GET: Administration/Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FirstName", book.AuthorId);
            ViewData["EditionLanguageId"] = new SelectList(_context.EditionLanguages, "Id", "Name", book.EditionLanguageId);
            return View(book);
        }

        // POST: Administration/Books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,AuthorId,Summary,PublishedOn,Pages,PictureURL,EditionLanguageId,AmazonLink,CreatedOn,ModifiedOn,IsDeleted,DeletedOn")] Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "FirstName", book.AuthorId);
            ViewData["EditionLanguageId"] = new SelectList(_context.EditionLanguages, "Id", "Name", book.EditionLanguageId);
            return View(book);
        }

        // GET: Administration/Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.EditionLanguage)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Administration/Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }*/
    }
}
