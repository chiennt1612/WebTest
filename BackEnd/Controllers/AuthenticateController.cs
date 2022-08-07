using BackEnd.ExceptionHandling;
using BackEnd.Helper;
using BackEnd.Models;
using BackEnd.Services.Interfaces;
using EntityFramework.API.Entities.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackEnd.Controllers
{
    [Route("api/v1.0/[controller]")]
    [ApiController]
    [TypeFilter(typeof(ControllerExceptionFilterAttribute))]
    [Produces("application/json", "application/problem+json")]
    public class AuthenticateController : ControllerBase
    {
        private readonly IUserClaimsPrincipalFactory<AppUser> _userClaimsPrincipalFactory;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ITokenCreationService _jwtToken;
        private readonly ILogger<AuthenticateController> _logger;
        public AuthenticateController(
            ILogger<AuthenticateController> logger,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration,
            ITokenCreationService jwtToken,
            IUserClaimsPrincipalFactory<AppUser> userClaimsPrincipalFactory)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
            _jwtToken = jwtToken;
            _signInManager = signInManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            _logger.LogInformation($"ModelState: {ModelState.IsValid}");
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"Finding username: {model.Username}");
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    _logger.LogInformation($"Found: {model.Username}");
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, lockoutOnFailure: true);
                    if (result.Succeeded)
                    {
                        return await LoginOK(user);
                    }
                    else if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return StatusCode(StatusCodes.Status200OK,
                                    new ResponseBase(Language.EntityValidation.Fail, $"{model.Username}: {Language.EntityValidation.AccountLockout}!", $"{model.Username}: {Language.EntityValidation.AccountLockout}!", 0, 400));
                    }
                }
            }

            _logger.LogInformation($"Not found: {model.Username}");
            _logger.LogInformation($"Not found: {model.Username}");
            return StatusCode(StatusCodes.Status200OK, new ResponseOK()
            {
                Code = 400,
                InternalMessage = Language.EntityValidation.NotFound,
                MoreInfo = Language.EntityValidation.NotFound,
                Status = 0,
                UserMessage = Language.EntityValidation.NotFound,
                data = null
            });
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            _logger.LogInformation($"ModelState: {ModelState.IsValid}");
            if (ModelState.IsValid)
            {
                var userExists = await _userManager.FindByNameAsync(model.Username);
                if (userExists == null)
                {
                    AppUser user = new AppUser()
                    {
                        UserName = model.Username,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        TwoFactorEnabled = false
                    };
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        return await LoginOK(user);
                    }
                }
                else
                {
                    _logger.LogInformation($"Found: {model.Username}");
                    var result = await _signInManager.PasswordSignInAsync(userExists.UserName, model.Password, false, lockoutOnFailure: true);
                    if (result.Succeeded)
                    {
                        return await LoginOK(userExists);
                    }
                    else if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account locked out.");
                        return StatusCode(StatusCodes.Status200OK,
                                    new ResponseBase(Language.EntityValidation.Fail, $"{model.Username}: {Language.EntityValidation.AccountLockout}!", $"{model.Username}: {Language.EntityValidation.AccountLockout}!", 0, 400));
                    }
                }
            }
            _logger.LogError($"{model.Username}: {Language.EntityValidation.UserCreateFail}");
            return StatusCode(StatusCodes.Status200OK,
                new ResponseBase(Language.EntityValidation.UserCreateFail, Language.EntityValidation.UserCreateFail, Language.EntityValidation.UserCreateFail));
        }

        private async Task<IActionResult> LoginOK(AppUser user)
        {
            _logger.LogInformation($"Logined: {user.UserName}");
            ClaimsPrincipal userClaims = await _userClaimsPrincipalFactory.CreateAsync(user);
            List<Claim> claims = userClaims.Claims.ToList();

            claims.Add(new Claim("username", user.UserName));
            claims.Add(new Claim("aud", _configuration["JWT:ValidAudience"]));

            var token = await _jwtToken.CreateTokenAsync(_jwtToken.CreateAccessTokenAsync(claims));
            var refreshToken = _jwtToken.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
            _logger.LogInformation($"RefreshToken: {refreshToken}\nToken: {token}\n");
            await _userManager.UpdateAsync(user);
            
            return Ok(new ResponseOK()
            {
                Status = 1,
                UserMessage = "Login success!",
                InternalMessage = $"{user.UserName}: Login success!",
                Code = 200,
                MoreInfo = $"Login success!",
                data = new {
                    token = token,
                    refreshToken = refreshToken
                }
            });
        }
    }
}
