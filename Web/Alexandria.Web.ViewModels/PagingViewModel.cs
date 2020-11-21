namespace Alexandria.Web.ViewModels
{
    public abstract class PagingViewModel
    {
        public int Id { get; set; }

        public int CurrentPage { get; set; }

        public int PagesCount { get; set; }

        public bool HasPreviousPage => this.CurrentPage > 1;

        public bool HasNextPage => this.CurrentPage < this.PagesCount;

        public string ControllerName { get; set; }

        public string ActionName { get; set; }
    }
}
