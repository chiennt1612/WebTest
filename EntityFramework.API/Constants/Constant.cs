using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFramework.API.Constants
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string User = "User";
    }

    public static class TableConsts
    {
        public const int PageSize = 10;

        public const string Movies = "Movies";
        public const string MoviesShare = "MovieShare";
    }

    public static class CommonFields
    {
        public const string CreatedBy = "UserCreator";
        public const string CreatedOn = "DateCreator";
        public const string UpdatedBy = "UserModify";
        public const string UpdatedOn = "DateModify";
        public const string IsDeleted = "IsDeleted";
        public const string UserDeleted = "UserDeleted";
        public const string DateDeleted = "DateDeleted";
    }
}
