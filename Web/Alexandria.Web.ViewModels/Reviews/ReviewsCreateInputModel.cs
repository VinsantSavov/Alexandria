namespace Alexandria.Web.ViewModels.Reviews
{
    using System.ComponentModel.DataAnnotations;

    using Alexandria.Data.Models;
    using Alexandria.Data.Models.Enums;
    using Alexandria.Services.Mapping;
    using AutoMapper;

    public class ReviewsCreateInputModel : IMapFrom<Book>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string PictureURL { get; set; }

        public int AuthorId { get; set; }

        public string Author { get; set; }

        [Display(Name = "What did you think?")]
        [DataType(DataType.MultilineText)]
        [Required]
        [MinLength(20)]
        public string Description { get; set; }

        [Display(Name = "What is your reading progress?")]
        public ReadingProgress ReadingProgress { get; set; }

        [Display(Name = "Is this the edition you read?")]
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
