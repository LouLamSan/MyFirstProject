using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Api.Dtos;
using WebApplication.Api.Services;

namespace WebApplication.Api.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        [HttpGet]
        public JsonResult GetProduct()
        {
            return new JsonResult(ProductService.Current.Products);
        }

        [Route("{id}")]
        public JsonResult GetProductByID(int id)
        {
            return new JsonResult(ProductService.Current.Products.SingleOrDefault(p => p.ID == id));
        }
    }
}
