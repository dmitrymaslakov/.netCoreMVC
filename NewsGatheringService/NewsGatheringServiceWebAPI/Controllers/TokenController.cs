using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsGatheringService.BLL.Interfaces;
using NewsGatheringService.Models.BLL;
using NewsGatheringServiceWebAPI.Commands;

namespace NewsGatheringServiceWebAPI.Controllers
{
    /// <summary>
    /// Account controller for user registration and authorization.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<TokenController> _logger;
        private readonly IMediator _mediator;

        public TokenController(IUserService userService, ILogger<TokenController> logger, IMediator mediator)
        {
            _userService = userService;
            _logger = logger;
            _mediator = mediator;
        }


        /// <summary>
        /// Refresh token based on RefreshToken stored in the request cookies
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];

                var res = await _mediator.Send(new RefreshTokenCommand(refreshToken));

                if (res == null)
                {
                    return BadRequest(new { message = "Invalid user or password" });
                }
                
                SetCookieToken(res.RefreshToken);
                
                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                
                return StatusCode(500);
            }

        }

        private void SetCookieToken(string token)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7),
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

    }
}
