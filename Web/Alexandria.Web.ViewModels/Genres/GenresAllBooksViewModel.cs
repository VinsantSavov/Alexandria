﻿namespace Alexandria.Web.ViewModels.Genres
{
    using System.Collections.Generic;

    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class GenresAllBooksViewModel : PagingViewModel, IMapFrom<Genre>
    {
        public string Name { get; set; }

        public IEnumerable<GenresBookDetailsViewModel> AllBooks { get; set; }
    }
}
