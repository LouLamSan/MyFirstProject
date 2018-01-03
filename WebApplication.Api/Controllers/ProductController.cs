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

        [Route("{id}", Name = "GetProduct")]
        public IActionResult GetProductByID(int id)
        {
            var product = ProductService.Current.Products.SingleOrDefault(p => p.ID == id);
            if(product==null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Post([FromBody] ProductCreation product)
        {
            if(product == null)
            {
                return BadRequest();
            }

            if (product.Name == "产品")
            {
                ModelState.AddModelError("Name", "产品的名称不可以是'产品'二字");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var maxId = ProductService.Current.Products.Max(p => p.ID);

            var newProduct = new Product
            {
                ID = ++maxId,
                Name = product.Name,
                Price = product.Price
            };

            ProductService.Current.Products.Add(newProduct);

            return CreatedAtRoute("GetProduct", new { id = newProduct.ID }, newProduct);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id,[FromBody] ProductModification product)
        {
            if(product == null)
            {
                return BadRequest();
            }

            if (product.Name == "产品")
            {
                ModelState.AddModelError("Name", "产品的名称不可以是'产品'二字");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = ProductService.Current.Products.SingleOrDefault(p => p.ID == id);

            if(model == null)
            {
                return NotFound();
            }

            model.Name = product.Name;
            model.Price = product.Price;

            return NoContent();
        }
    }
}
