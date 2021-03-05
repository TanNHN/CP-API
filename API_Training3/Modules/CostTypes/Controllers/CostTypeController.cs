using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API_Training3.App.Controllers;
using API_Training3.App.Helper;
using API_Training3.Modules.CostTypes.Entities;
using API_Training3.Modules.CostTypes.Requests;
using API_Training3.Modules.CostTypes.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API_Training3.Modules.CostTypes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CostTypeController : BaseController
    {

        private readonly ICostTypeServices _costTypeServices;

        public CostTypeController(ICostTypeServices costTypeService)
        {
            _costTypeServices = costTypeService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return ResponseOk(_costTypeServices.GetAll(), "Success");
        }

  /*      [HttpGet("{id}")]
        public IActionResult GetDetail(string id)
        {
            (object data, string message) = _costTypeServices.GetById(id);
            if (data == null)
            {
                return ResponseBadRequest(message);
            }
            return ResponseOk(data, message);
        }*/


        [HttpPost]
        public IActionResult Store([FromBody] CostTypeRequest request)
        {
            return ResponseOk(_costTypeServices.Store(request), "Cost type insert success");
        }

        [HttpPost("Search")]
        public IActionResult Search([FromBody] SearchRequest request)
        {
            return ResponseOk(_costTypeServices.Search(request), "Success");
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] CostTypeRequest request, string id)
        {
            (Object data, string message) = _costTypeServices.Update(request, id);
            if (data == null)
            {
                return ResponseBadRequest(message);
            }
            return ResponseOk(data, message);
        }
    }
}

