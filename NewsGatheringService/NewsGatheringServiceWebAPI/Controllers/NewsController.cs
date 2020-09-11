using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace NewsGatheringServiceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;

        public NewsController(ILogger<NewsController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Returns all the news in the database
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public IActionResult GetNews()
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }
        /// <summary>
        /// Returns all the news in the database by category/subcategory 
        /// </summary>
        /// <returns></returns>
        [HttpGet("{category}/{subcategory}")]
        public IActionResult GetNewsByCategory()
        {
            try
            {

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }
    }
}
