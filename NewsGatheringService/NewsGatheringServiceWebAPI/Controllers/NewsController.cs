﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsGatheringService.Core.Abstract;
using NewsGatheringService.Data.Entities;
using NewsGatheringServiceWebAPI.Models;
using NewsGatheringServiceWebAPI.Queries;

namespace NewsGatheringServiceWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public NewsController(ILogger<NewsController> logger, IMediator mediator, IMapper mapper, IUserService userService)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _userService = userService;
        }
        /// <summary>
        /// Returns all the news in the database
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetNews()
        {
            try
            {
                var res = await _mediator.Send(new GetNewsQuery());
                var model = _mapper.Map<IEnumerable<NewsDTO>>(res);
                return Ok(model);
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
        [HttpGet("{categoryName?}/{subcategoryName?}")]
        public async Task<IActionResult> GetNewsByCategory(string categoryName, string subcategoryName)
        {
            try
            {
                var res = await _mediator.Send(new GetNewsQuery(categoryName, subcategoryName));
                var model = _mapper.Map<IEnumerable<NewsDTO>>(res);
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }

        }

    }
}