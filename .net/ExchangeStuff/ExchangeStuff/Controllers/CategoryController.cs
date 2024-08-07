﻿using ExchangeStuff.AuthOptions.Requirements;
using ExchangeStuff.Service.Constants;
using ExchangeStuff.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeStuff.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService) 
        {
            _categoryService = categoryService;
        }
        [ESAuthorize(new string[] { ActionConstant.READ })]

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var category = await _categoryService.GetCategory();
            return Ok(category);
        }
    }
}
