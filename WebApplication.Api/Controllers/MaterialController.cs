using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Api.Services;

namespace WebApplication.Api.Controllers
{
    [Route("api/product")]
    public class MaterialController : Controller
    {
        [HttpGet("{productId}/materials")]
        public IActionResult GetMaterials(int productId)
        {
            var product = ProductService.Current.Products.SingleOrDefault(p => p.ID == productId);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product.Materials);
        }

        [HttpGet("{productId}/materials/{id}")]
        public IActionResult GetMaterial(int productId,int id)
        {
            var product = ProductService.Current.Products.SingleOrDefault(p => p.ID == productId);
            if(product == null)
            {
                return NotFound();
            }
            var material = product.Materials.SingleOrDefault(m => m.ID == id);
            if(material == null)
            {
                return NotFound();
            }

            return Ok(material);
        }
    }
}
