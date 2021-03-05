using API_Training3.App.Controllers;
using API_Training3.Entities;
using API_Training3.Modules.Categories.Requests;
using API_Training3.Modules.Categories.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Training3.Modules.Categories.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController
    {

        private readonly ICategoryService _categoryservice;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryService categoryservice, IMapper mapper)
        {
            _categoryservice = categoryservice;
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            return ResponseOk(_categoryservice.GetAll(), "Success");
        }

        [HttpGet("{id}")]
        public IActionResult GetDetail(string id)
        {
            (object data, string message) = _categoryservice.GetById(id);
            if (data == null)
            {
                return ResponseBadRequest(message);
            }
            return ResponseOk(data, message);
        }


        [HttpPost]
        [Authorize]
        public IActionResult Store([FromBody] StoreCategoryRequest request)
        {
            return ResponseOk(_categoryservice.Store(request), "Success");
        }

        [HttpPost("Search")]
        public IActionResult Search([FromBody] SearchCategoryRequest request)
        {
            return ResponseOk(_categoryservice.Search(request), "Success");
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] UpdateCategoryRequest request, string id)
        {
            (Category category, string message) = _categoryservice.Update(request, id);
            if (category == null)
            {
                return ResponseBadRequest(message);
            }
            return ResponseOk(category, message);
        }
    }
}
