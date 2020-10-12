using System;
using NewsGatheringService.BLL.DTO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsGatheringServiceWebAPI.Queries;
using Microsoft.AspNetCore.Authorization;
using NewsGatheringService.Models.BLL;
using NewsGatheringServiceWebAPI.Commands;

namespace NewsGatheringServiceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public UserController(ILogger<NewsController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        //api/user/id
        /// <summary>
        /// Returns the user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Return the user by id or BadRequest if user null</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var res = await _mediator.Send(new GetUserByIdQuery(id));

                if (res == null)
                    return BadRequest(new { message = "This user is not in the database" });

                var model = _mapper.Map<UserDTO>(res);

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500);
            }

        }

        //api/user/registrate
        /// <summary>
        /// User registration. If registration is successful, authentication is performed automatically. 
        /// </summary>
        /// <param name="request">Contains login and password required for registration</param>
        /// <returns>The payload an authenticated user's is returned or BadRequest if the user already exists in db.</returns>
        [AllowAnonymous]
        [HttpPost("registrate")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var res = await _mediator.Send(new RegistrateUserCommand(request));
                
                if (res == null)
                    return BadRequest(new { message = "User already exists" });

                return Ok(res);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        //api/user/authenticate
        /// <summary>
        /// User authenticate
        /// </summary>
        /// <param name="request">Contains login and password required for authentication</param>
        /// <returns>The payload an authenticated user's is returned or BadRequest if the user has entered incorrect data.</returns>
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequest request)
        {
            try
            {
                var res = await _mediator.Send(new AuthenticateUserCommand(request));
                
                if (res == null)
                    return BadRequest(new { message = "Username or password is incorrect" });

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
