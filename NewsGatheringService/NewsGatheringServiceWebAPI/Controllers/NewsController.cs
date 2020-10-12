using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsGatheringService.BLL.DTO;
using NewsGatheringService.BLL.Interfaces;
using NewsGatheringServiceWebAPI.Commands;
using NewsGatheringServiceWebAPI.Queries;

namespace NewsGatheringServiceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public NewsController(ILogger<NewsController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        //api/news/id
        /// <summary>
        /// Returns the one news by id
        /// </summary>
        /// <param name="id">News id</param>
        /// <returns>Returns the news by id or BadRequest if news null</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(Guid id)
        {
            try
            {
                var res = await _mediator.Send(new GetOneNewsQuery(id));
                
                if (res == null)
                    return BadRequest(new { message = "This news is not in the database" });

                var model = _mapper.Map<NewsDTO>(res);
                
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                
                return StatusCode(500);
            }
        }

        //api/news
        /// <summary>
        /// Returns all news in the database. The rate, category and subcategory are optional parameters.
        /// </summary>
        /// <param name="rate"></param>
        /// <param name="categoryName"></param>
        /// <param name="subcategoryName"></param>
        /// <returns>Returns all news based on the specified parameters or BadRequest if news null</returns>
        [HttpGet()]
        public async Task<IActionResult> Get(int? rate = null, string categoryName = "", string subcategoryName = "")
        {
            try
            {
                var res = await _mediator.Send(new GetNewsQuery(rate, categoryName, subcategoryName));
                
                if (res == null)
                    return BadRequest(new { message = "There are no news in the database" });

                var model = _mapper.Map<IEnumerable<NewsDTO>>(res);
                
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                
                return StatusCode(500);
            }
        }
        
        /// <summary>
        /// Add news ulrs to database.
        /// </summary>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<IActionResult> AddNewsUrlsToDb([FromBody] string[] latestNewsUrls)
        {
            try
            {
                await _mediator.Send(new AddNewsUrlsCommand(latestNewsUrls));
                
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500);
            }
        }

        /// <summary>
        /// Parse news and insert them into database
        /// </summary>
        /// <returns></returns>
        [HttpPost("parse")]
        public async Task<IActionResult> ParseNews()
        {
            try
            {
                await _mediator.Send(new ParseNewsCommand());

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500);
            }
        }

        /// <summary>
        /// Rate news.
        /// </summary>
        /// <returns></returns>
        [HttpPost("rate")]
        public async Task<IActionResult> RateNews()
        {
            try
            {
                await _mediator.Send(new RateNewsCommand());

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500);
            }
        }

        /// <summary>
        /// Display an index of the latest news.
        /// </summary>
        /// <returns></returns>
        [HttpGet("latest")]
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
                
                return StatusCode(500);
            }

        }
    }
}
