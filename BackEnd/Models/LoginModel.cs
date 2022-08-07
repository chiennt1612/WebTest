using System.ComponentModel.DataAnnotations;

namespace BackEnd.Models
{
    public class LoginModel
    {
        [Display(Name = "Username", ResourceType = typeof(Language.EntityValidation))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Language.EntityValidation))]
        public string? Username { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Language.EntityValidation))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Language.EntityValidation))]
        public string? Password { get; set; }
    }

    public class RegisterModel
    {
        [Display(Name = "Username", ResourceType = typeof(Language.EntityValidation))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Language.EntityValidation))]
        public string? Username { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Language.EntityValidation))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Language.EntityValidation))]
        public string? Password { get; set; }
    }

    public class RefreshTokenModel
    {
        [Display(Name = "Token", ResourceType = typeof(Language.EntityValidation))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Language.EntityValidation))]
        public string? Token { get; set; }
    }
}
