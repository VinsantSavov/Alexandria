namespace Alexandria.Web.Areas.Administration.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Alexandria.Common;
    using Alexandria.Services.Tags;
    using Alexandria.Web.ViewModels.Administration.Tags;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    public class TagsController : AdministrationController
    {
        private const int TagsPerPage = 10;
        private const string ControllerName = "Tags";
        private const string TempDataMessageConstant = "You have successfully added a tag!";

        private readonly ITagsService tagsService;

        public TagsController(ITagsService tagsService)
        {
            this.tagsService = tagsService;
        }

        // GET: Administration/Tags
        public async Task<IActionResult> Index(int page = 1)
        {
            var viewModel = new ATagsAllViewModel();

            var tagsCount = await this.tagsService.GetTagsCountAsync();

            viewModel.Tags = await this.tagsService.GetAllTagsAsync<ATagsSingleViewModel>(TagsPerPage, (page - 1) * TagsPerPage);
            viewModel.CurrentPage = page;
            viewModel.PagesCount = (int)Math.Ceiling((double)tagsCount / TagsPerPage);
            viewModel.ControllerName = ControllerName;
            viewModel.ActionName = nameof(this.Index);

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ATagsCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.tagsService.CreateTagAsync(input.Name);

            this.TempData[GlobalConstants.TempDataName] = TempDataMessageConstant;
            return this.RedirectToAction(nameof(this.Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return this.NotFound();
            }

            var tag = await this.tagsService.GetTagByIdAsync<ATagsDeleteViewModel>(id.Value);

            if (tag == null)
            {
                return this.NotFound();
            }

            return this.View(tag);
        }

        [HttpPost]
        [ActionName(nameof(Delete))]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doExist = await this.tagsService.DoesTagIdExistAsync(id);

            if (!doExist)
            {
                return this.NotFound();
            }

            await this.tagsService.DeleteTagByIdAsync(id);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
