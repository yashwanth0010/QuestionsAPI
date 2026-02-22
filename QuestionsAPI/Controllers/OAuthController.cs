using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using QuestionsAPI.DTO;
using QuestionsAPI.Services;
using System.Linq;
using System.Collections.Generic;

namespace QuestionsAPI.Controllers
{
    [ApiController]
    [Route("oauth")]
    public class OAuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public OAuthController(ITokenService tokenService, IConfiguration config)
        {
            _tokenService = tokenService;
            _config = config;
        }

        [HttpPost("token")]
        public IActionResult Token([FromBody] TokenRequest request)
        {
            var oauthClients = _config.GetSection("OAuthClients").Get<List<OAuthClient>>();
            var client = oauthClients?.FirstOrDefault(c => c.ClientId == request.ClientId);

            if (client == null || client.ClientSecret != request.ClientSecret)
            {
                return Unauthorized();
            }

            if (request.GrantType != "client_credentials")
            {
                return BadRequest(new { error = "unsupported_grant_type" });
            }

            var token = _tokenService.GenerateToken(request.ClientId);
            var jwtSettings = _config.GetSection("JwtSettings");
            var expiresIn = int.Parse(jwtSettings["ExpirationMinutes"]) * 60;

            return Ok(new TokenResponse
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = expiresIn
            });
        }
    }
}
