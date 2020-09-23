using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsGatheringServiceWebAPI.Commands;
using NewsGatheringServiceWebAPI.Queries;

namespace NewsGatheringServiceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IMediator _mediator;

        public AdminController(ILogger<AdminController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        /// <summary>
        /// Add latest news to database.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> AddLatestNewsToDb([FromBody] string[] latestNewsUrls)
        {
            try
            {
                return await _mediator.Send(new AddNewsCommand(latestNewsUrls));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }
        /// <summary>
        /// Displays an index of the latest news.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> LatestNewsIndex()
        {
            try
            {
                var res = await _mediator.Send(new IndexLatestNewsQuery());
                return Ok(res.ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }
    }
}
