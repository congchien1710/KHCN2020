using KHCN.Api.Services;
using KHCN.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KHCN.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class TokenController : BaseController<TokenController>
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;

        public TokenController(ITokenService tokenService, IUserRepository userRepository)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Refresh(string token, string refreshToken)
        {
            try
            {
                sw.Restart();
                var principal = _tokenService.GetPrincipalFromExpiredToken(token);
                var username = principal.FindFirst("Username").Value;

                var user = _userRepository.GetAll().SingleOrDefault(u => u.UserName == username);
                if (user == null || user.RefreshToken != refreshToken) return BadRequest();

                var newJwtToken = _tokenService.GenerateAccessToken(principal.Claims);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                await _userRepository.UpdateAsync(user, user.Id);
                sw.Stop();
                log.LogInformation($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[Token.Refresh]. Refresh token refreshToken = {refreshToken} success. Time process: {sw.Elapsed.Seconds} seconds.");

                return new ObjectResult(new
                {
                    token = newJwtToken,
                    refreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
                sw.Stop();
                log.LogError($"[{HttpContext.Connection.RemoteIpAddress.ToString()}]-[Token.Refresh]. Refresh token refreshToken = {refreshToken} error. Detail: {ex}");
                return BadRequest();
            }
        }

        [HttpPost, Authorize]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity.Name;

            var user = _userRepository.GetByUsername(username);
            if (user == null)
                return BadRequest();

            user.RefreshToken = null;

            await _userRepository.UpdateAsync(user, user.Id);

            return NoContent();
        }
    }
}