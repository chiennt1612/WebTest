using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    public class MovieShareModel
    {
        [StringLength(300, ErrorMessageResourceName = "StringLengthTooLong", ErrorMessageResourceType = typeof(Language.EntityValidation))]
        [Display(Name = "Title", ResourceType = typeof(Language.EntityValidation))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Language.EntityValidation))]
        public string Title { get; set; }

        [StringLength(300, ErrorMessageResourceName = "StringLengthTooLong", ErrorMessageResourceType = typeof(Language.EntityValidation))]
        [Display(Name = "Link", ResourceType = typeof(Language.EntityValidation))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Language.EntityValidation))]
        public string Link { get; set; }

        [StringLength(2000, ErrorMessageResourceName = "StringLengthTooLong", ErrorMessageResourceType = typeof(Language.EntityValidation))]
        [Display(Name = "Description", ResourceType = typeof(Language.EntityValidation))]
        public string? Description { get; set; }

        public bool IsPublish { get; set; } = false;

        public List<long>? UserIds { get; set; }
    }

    public enum GetListBy
    {
        MyVideo = 0,
        ShareVideo = 1
    }
}
