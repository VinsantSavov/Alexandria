namespace Alexandria.Web.InputModels.Reviews
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Data.Models.Enums;

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
