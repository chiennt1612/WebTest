using EntityFramework.API.Entities;
using EntityFramework.API.Entities.EntityBase;

namespace BackEnd.Models
{
    public class ResponseOK
    {
        public int? Status { get; set; }
        public int? Code { get; set; }
        public string? UserMessage { get; set; }
        public string? InternalMessage { get; set; }
        public string? MoreInfo { get; set; }
    }

    public class MoviesList : ResponseOK
    {
        public BaseEntityList<Movies>? data { get; set; }
    }

    public class TokenModel
    {
        public string? Token { get; set; }

        public string? RefreshToken { get; set; }
        public long? UserId { get; set; }
        public string? UserName { get; set; }
    }

    public class LoginOutputModel : ResponseOK
    {
        public TokenModel? data { get; set; }
    }
}
