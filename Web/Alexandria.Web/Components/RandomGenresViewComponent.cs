namespace Alexandria.Web.Components
{
    using System.Threading.Tasks;

    using Alexandria.Services.Genres;
    using Alexandria.Web.ViewModels.Genres;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent(Name = "RandomGenres")]
    public class RandomGenresViewComponent : ViewComponent
    {
        private const int Count = 8;

        private readonly IGenresService genresService;

        public RandomGenresViewComponent(IGenresService genresService)
        {
            this.genresService = genresService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var genres = await this.genresService.GetRandomGenresAsync<GenresRandomViewModel>(Count);

            return this.View(genres);
        }
    }
}
