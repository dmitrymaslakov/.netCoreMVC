using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsGatheringService.BLL.DTO;
using NewsGatheringService.BLL.Interfaces;
using NewsGatheringServiceWebAPI.Queries;

namespace NewsGatheringServiceWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public SubcategoryController(ILogger<NewsController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        //api/subcategory/name
        /// <summary>
        /// Returns the news subcategory by its name
        /// </summary>
        /// <param name="name">Subcategory name</param>
        /// <returns>Returns the news subcategory by its name or BadRequest if subcategory null</returns>
        [HttpGet("{name}")]
        public async Task<IActionResult> GetOne(string name)
        {
            try
            {
                var res = await _mediator.Send(new GetSubcategoryQuery(name));
                
                if (res == null)
                    return BadRequest(new { message = "This category is not in the database" });

                var model = _mapper.Map<SubcategoryDTO>(res);
                
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                
                return StatusCode(500);
            }
        }

        //api/subcategory/categoryName
        /// <summary>
        /// Returns all subcategories in the database. The categoryName is optional parameter.
        /// </summary>
        /// <param categoryName="categoryName"></param>
        /// <returns>Returns all subcategories based on the specified parameters or BadRequest if subcategories null</returns>
        [HttpGet("byCategory")]
        public async Task<IActionResult> Get(string categoryName = null)
        {
            try
            {
                var res = await _mediator.Send(new GetSubcategoriesQuery(categoryName));

                if (res == null)
                    return BadRequest(new { message = "There are no categories in the database" });

                var model = _mapper.Map<IEnumerable<SubcategoryDTO>>(res);

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
