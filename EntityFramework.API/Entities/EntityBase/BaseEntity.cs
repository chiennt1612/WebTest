using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EntityFramework.API.Entities.EntityBase
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Display(Name = "UserCreator", ResourceType = typeof(Language.EntityValidation))]
        public long UserCreator { get; set; }
        public string? EmailCreator { get; set; }
        [Display(Name = "DateCreator", ResourceType = typeof(Language.EntityValidation))]
        public DateTime DateCreator { get; set; }
        [Display(Name = "UserModify", ResourceType = typeof(Language.EntityValidation))]
        public long? UserModify { get; set; }
        [Display(Name = "DateModify", ResourceType = typeof(Language.EntityValidation))]
        public DateTime? DateModify { get; set; }

        [JsonIgnore]
        public bool IsDeleted { get; set; }
        [Display(Name = "UserDeleted", ResourceType = typeof(Language.EntityValidation))]
        [JsonIgnore]
        public long? UserDeleted { get; set; }
        [Display(Name = "DateDeleted", ResourceType = typeof(Language.EntityValidation))]
        [JsonIgnore]
        public DateTime? DateDeleted { get; set; }
    }
}
