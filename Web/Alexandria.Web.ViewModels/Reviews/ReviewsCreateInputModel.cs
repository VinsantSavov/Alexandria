namespace Alexandria.Web.ViewModels.Reviews
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Common;
    using Alexandria.Data.Models;
    using Alexandria.Data.Models.Enums;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class ReviewsCreateInputModel : IMapFrom<Book>, IHaveCustomMappings
    {
        [Required]
        public int Id { get; set; }

        public string Title { get; set; }

        public string PictureURL { get; set; }

        public int AuthorId { get; set; }

        public string Author { get; set; }

        [Display(Name = GlobalConstants.ReviewDescriptionDisplayNameConstant)]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = ErrorMessages.ReviewRequiredDescriptionErrorMessage)]
        [StringLength(
            GlobalConstants.ReviewDescriptionMaxLength,
            ErrorMessage = ErrorMessages.ReviewDescriptionLengthErrorMessage,
            MinimumLength = GlobalConstants.ReviewDescriptionMinLength)]
        public string Description { get; set; }

        [Required]
        [Display(Name = GlobalConstants.ReviewReadingProgressDisplayNameConstant)]
        [EnumDataType(typeof(ReadingProgress))]
        public ReadingProgress ReadingProgress { get; set; }

        [Required]
        [Display(Name = GlobalConstants.ReviewThisEditionDisplayNameConstant)]
        public bool ThisEdition { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Book, ReviewsCreateInputModel>()
                .ForMember(
                r => r.Author,
                b => b.MapFrom(src => src.Author.FirstName + " " + src.Author.LastName));
        }
    }
}
