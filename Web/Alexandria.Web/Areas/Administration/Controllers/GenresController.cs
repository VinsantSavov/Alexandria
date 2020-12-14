namespace Alexandria.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Data;
    using Alexandria.Data.Models;
    using Alexandria.Services.Genres;
    using Alexandria.Web.ViewModels.Administration.Genres;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    public class GenresController : AdministrationController
    {
        private const int GenresPerPage = 10;
        private const string ControllerName = "Genres";

        private readonly IGenresService genresService;

        public GenresController(IGenresService genresService)
        {
            this.genresService = genresService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var viewModel = new GenresAllGenresViewModel();

            var genresCount = await this.genresService.GetGenresCountAsync();

            viewModel.Genres = await this.genresService.GetAllGenresAsync<GenresSingleGenreViewModel>(GenresPerPage, (page - 1) * GenresPerPage);
            viewModel.PagesCount = (int)Math.Ceiling((double)genresCount / GenresPerPage);
            viewModel.CurrentPage = page;
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

            var genre = await this.genresService.GetGenreByIdAsync<GenresDetailsViewModel>(id.Value);

            if (genre == null)
            {
                return this.NotFound();
            }

            return this.View(genre);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(GenresCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var id = await this.genresService.CreateGenreAsync(input.Name, input.Description);

            return this.RedirectToAction(nameof(this.Details), new { Id = id });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var genre = await this.genresService.GetGenreByIdAsync<GenresEditInputModel>(id.Value);

            if (genre == null)
            {
                return this.NotFound();
            }

            return this.View(genre);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(GenresEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.genresService.EditGenreAsync(input.Id, input.Name, input.Description);

            return this.RedirectToAction(nameof(this.Details), new { Id = input.Id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var genre = await this.genresService.GetGenreByIdAsync<GenresDeleteViewModel>(id.Value);

            if (genre == null)
            {
                return this.NotFound();
            }

            return this.View(genre);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doExist = await this.genresService.DoesGenreIdExistAsync(id);

            if (!doExist)
            {
                return this.NotFound();
            }

            await this.genresService.DeleteGenreByIdAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
