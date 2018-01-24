using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ProductController> _logger;
        private readonly LocalMailService _localMailService;

        public ProductController(ILogger<ProductController> logger,LocalMailService localMailService)
        {
            _logger = logger;
            _localMailService = localMailService;
        }

        [HttpGet]
        public IActionResult GetProduct()
        {
            return Ok(ProductService.Current.Products);
        }

        [Route("{id}", Name = "GetProduct")]
        public IActionResult GetProductByID(int id)
        {
            try
            {
                
                var product = ProductService.Current.Products.SingleOrDefault(p => p.ID == id);
                if (product == null)
                {
                    _logger.LogInformation($"ID为 {id} 的产品没有被找到...");
                    return NotFound();
                }
                return Ok(product);
            }
            catch(Exception ex)
            {
                _logger.LogCritical($"查找Id为{id}的产品时出现了错误!!", ex);
                return StatusCode(500, "处理请求的时候发生了错误！");
            }
            
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
                Price = product.Price,
                Description = product.Description
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
            model.Description = product.Description;

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id,[FromBody] JsonPatchDocument<ProductModification> patchDoc)
        {
            if(patchDoc == null)
            {
                return BadRequest();
            }

            var model = ProductService.Current.Products.SingleOrDefault(p => p.ID == id);

            if(model == null)
            {
                return NotFound();
            }

            var toPatch = new ProductModification
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price
            };

            patchDoc.ApplyTo(toPatch, ModelState);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(toPatch.Name == "产品")
            {
                ModelState.AddModelError("Name", "产品的名称不可以是'产品'二字");
            }
            TryValidateModel(toPatch);

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            model.Name = toPatch.Name;
            model.Description = toPatch.Description;
            model.Price = toPatch.Price;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var model = ProductService.Current.Products.SingleOrDefault(p => p.ID == id);

            if(model == null)
            {
                return NotFound();
            }

            ProductService.Current.Products.Remove(model);

            _localMailService.send("Product Deleted", $"Id为{id}的产品被删除了");

            return NoContent();
        }
    }
}
