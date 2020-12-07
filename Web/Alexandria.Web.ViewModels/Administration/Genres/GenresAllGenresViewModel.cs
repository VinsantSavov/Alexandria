namespace Alexandria.Web.ViewModels.Administration.Genres
{
    using System.Collections.Generic;

    public class GenresAllGenresViewModel : PagingViewModel
    {
        public IEnumerable<GenresSingleGenreViewModel> Genres { get; set; }
    }
}
