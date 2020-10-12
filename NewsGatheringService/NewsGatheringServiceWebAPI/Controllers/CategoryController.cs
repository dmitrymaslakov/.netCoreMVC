using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsGatheringService.BLL.DTO;
using NewsGatheringService.BLL.Interfaces;
/*using NewsGatheringService.CQS.DAL.Queries;
using NewsGatheringService.CQS.DAL.Handlers;*/
using NewsGatheringServiceWebAPI.Queries;


namespace NewsGatheringServiceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CategoryController(ILogger<NewsController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        //api/category/name
        /// <summary>
        /// Returns the news category by its name
        /// </summary>
        /// <param name="name">Category name</param>
        /// <returns>Returns the news category by its name or BadRequest if category null</returns>
        [HttpGet("{name}")]
        public async Task<IActionResult> GetOne(string name)
        {
            try
            {
                var res = await _mediator.Send(new GetCategoryQuery(name));
                
                if (res == null)
                    return BadRequest(new { message = "This category is not in the database" });

                var model = _mapper.Map<CategoryDTO>(res);
                
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                
                return StatusCode(500);
            }
        }

        //api/category
        /// <summary>
        /// Returns all categories in the database.
        /// </summary>
        /// <returns>Returns all categories or BadRequest if categories null</returns>
        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            try
            {
                var res = await _mediator.Send(new GetCategoriesQuery());

                if (res == null)
                    return BadRequest(new { message = "There are no categories in the database" });

                var model = _mapper.Map< IEnumerable<CategoryDTO>>(res);

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500);
            }
        }
    }
}
