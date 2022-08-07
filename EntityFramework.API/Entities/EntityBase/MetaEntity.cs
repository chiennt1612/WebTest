using System.ComponentModel.DataAnnotations;

namespace EntityFramework.API.Entities.EntityBase
{
    public class MetaEntity : BaseEntity
    {
        //[JsonIgnore]
        [Display(Name = "MetaTitle", ResourceType = typeof(Language.EntityValidation))]
        [StringLength(2000, ErrorMessageResourceName = "StringLengthTooLong", ErrorMessageResourceType = typeof(Language.EntityValidation))]
        public string? MetaTitle { get; set; }

        //[JsonIgnore]
        [Display(Name = "MetaDescription", ResourceType = typeof(Language.EntityValidation))]
        [StringLength(2000, ErrorMessageResourceName = "StringLengthTooLong", ErrorMessageResourceType = typeof(Language.EntityValidation))]
        public string? MetaDescription { get; set; }

        //[JsonIgnore]
        [Display(Name = "MetaKeyword", ResourceType = typeof(Language.EntityValidation))]
        [StringLength(2000, ErrorMessageResourceName = "StringLengthTooLong", ErrorMessageResourceType = typeof(Language.EntityValidation))]
        public string? MetaKeyword { get; set; }

        //[JsonIgnore]
        [Display(Name = "MetaBox", ResourceType = typeof(Language.EntityValidation))]
        [StringLength(2000, ErrorMessageResourceName = "StringLengthTooLong", ErrorMessageResourceType = typeof(Language.EntityValidation))]
        public string? MetaBox { get; set; }

        //[JsonIgnore]
        [Display(Name = "MetaRobotTag", ResourceType = typeof(Language.EntityValidation))]
        [StringLength(2000, ErrorMessageResourceName = "StringLengthTooLong", ErrorMessageResourceType = typeof(Language.EntityValidation))]
        public string? MetaRobotTag { get; set; }
    }
}
