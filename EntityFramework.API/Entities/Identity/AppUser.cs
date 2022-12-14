using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace EntityFramework.API.Entities.Identity
{
    public class AppUser : IdentityUser<long>
    {
        public int TotalOTP { get; set; }
        public DateTime? OTPSendTime { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
