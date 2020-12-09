namespace Alexandria.Web.ViewModels.Administration.Tags
{
    using System.Collections.Generic;

    public class ATagsAllViewModel : PagingViewModel
    {
        public IEnumerable<ATagsSingleViewModel> Tags { get; set; }
    }
}
