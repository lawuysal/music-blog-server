using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using music_blog_server.Models.DTO;
using music_blog_server.Repositories;

namespace music_blog_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Email,
                Email = registerRequestDto.Email,
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                identityResult = await userManager.AddToRoleAsync(identityUser, "ADMİN");

                if (identityResult.Succeeded)
                {
                    return Ok("Admin created");
                }
            }

            return BadRequest(identityResult.ToString());
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Email);

            if (user == null)
            {
                return BadRequest();
            }

            var isPassMatch =  await userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if(!isPassMatch)
            {
                return BadRequest();
            }

            var jwtToken = tokenRepository.CreateJWTToken(user);

            var response = new LoginResponseDto() { JWTToken = jwtToken };

            return Ok(response); 
        }
    }
}
