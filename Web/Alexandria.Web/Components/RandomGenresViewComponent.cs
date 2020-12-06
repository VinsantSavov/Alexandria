namespace Alexandria.Web.Components
{
    using System.Threading.Tasks;

    using Alexandria.Services.Genres;
    using Microsoft.AspNetCore.Mvc;

    [ViewComponent(Name = "RandomGenres")]
    public class RandomGenresViewComponent : ViewComponent
    {
        private readonly IGenresService genresService;

        public RandomGenresViewComponent(IGenresService genresService)
        {
            this.genresService = genresService;
        }
    }
}
