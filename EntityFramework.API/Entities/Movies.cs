using EntityFramework.API.Entities.EntityBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityFramework.API.Entities
{
    public class Movies : BaseEntity//: MetaEntity
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

        [Display(Name = "Like", ResourceType = typeof(Language.EntityValidation))]
        public int Like { get; set; }

        [Display(Name = "UnLike", ResourceType = typeof(Language.EntityValidation))]
        public int UnLike { get; set; }

        [JsonIgnore]
        [Display(Name = "IsPublish", ResourceType = typeof(Language.EntityValidation))]
        public bool IsPublish { get; set; }
    }
}
