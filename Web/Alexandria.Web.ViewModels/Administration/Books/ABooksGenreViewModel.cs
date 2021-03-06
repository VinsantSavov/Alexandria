﻿namespace Alexandria.Web.ViewModels.Administration.Books
{
    using Alexandria.Data.Models;
    using Alexandria.Services.Mapping;

    public class ABooksGenreViewModel : IMapFrom<Genre>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
