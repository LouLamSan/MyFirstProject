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
        public IActionResult GetProduct()
        {
            return Ok(ProductService.Current.Products);
        }

        [Route("{id}")]
        public IActionResult GetProductByID(int id)
        {
            var product = ProductService.Current.Products.SingleOrDefault(p => p.ID == id);
            if(product==null)
            {
                return NotFound();
            }
            return Ok(product);
        }
    }
}
