namespace Alexandria.Web.ViewModels.Home
{
    using System.Collections.Generic;

    public class HomeViewModel
    {
        public IEnumerable<HomeBookViewModel> TopBooks { get; set; }

        public IEnumerable<HomeBookViewModel> RandomBooks { get; set; }
    }
}
