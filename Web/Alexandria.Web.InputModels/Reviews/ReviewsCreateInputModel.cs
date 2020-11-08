using Alexandria.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Alexandria.Web.InputModels.Reviews
{
    public class ReviewsCreateInputModel
    {
        [Display(Name = "What did you think?")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "What is your reading progress?")]
        public ReadingProgress ReadingProgress { get; set; }

        [Display(Name = "Is this the edition you read?")]
        public bool ThisEdition { get; set; }
    }
}
