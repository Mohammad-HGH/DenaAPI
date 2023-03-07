using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DenaAPI.Models;
using DenaAPI.DTO;
using DenaAPI.Services;
using DenaAPI.Interfaces;

namespace DenaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController([FromForm] IProductService productService)
        {
            this.productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        [Route("ProductList")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var getProductResponse = await productService.GetAllProdsAsync();

            if (!getProductResponse.Success)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = "Not found",
                    ErrorCode = "S02"
                });
            }
            return Ok(getProductResponse);
        }

        // GET: api/Products/5
        [HttpGet]
        [Route("ViewProductById")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var getProductResponse = await productService.GetProdAsync(id);

            if (!getProductResponse.Success)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = "Not found",
                    ErrorCode = "S02"
                });
            }
            return Ok(getProductResponse);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut]
        [Route("UpdateProduct")]
        public async Task<IActionResult> PutProduct(int id, ProductRequest productRequest)
        {

            var getProductResponse = await productService.UpdateProdAsync(id, productRequest);

            if (!getProductResponse.Success)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = "Not found",
                    ErrorCode = "S02"
                });
            }
            return Ok(getProductResponse);
            /* var product1 = await _context.Products.FindAsync(id);
             if (product1 == null)
             {
                 return BadRequest();
             }

             product1.Name = product.Name;
             product1.CatId = product.CatId;

             try
             {
                 await _context.SaveChangesAsync();
             }
             catch (DbUpdateConcurrencyException)
             {
                 if (!ProductExists(id))
                 {
                     return NotFound();
                 }
                 else
                 {
                     throw;
                 }
             }

             return Ok();*/

        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("CreateProduct")]
        public async Task<ActionResult<Product>> PostProduct([FromForm] ProductRequest productRequest, [FromForm] AttributeRequest attributeRequest, [FromForm] long price)
        {
            var getProductResponse = await productService.CreateProdAsync(productRequest, attributeRequest, price);

            if (!getProductResponse.Success)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = "Not found",
                    ErrorCode = "S02"
                });
            }
            return Ok(getProductResponse);
            /*var existingProduct = await _context.Products.SingleOrDefaultAsync(
                product => product.CatId == productRequest.CatId
                && product.Name == productRequest.Name
                );
            var existingAttribute = await _context.Attributes.SingleOrDefaultAsync(
                    attribute => attribute.Type == attributeRequest.Type
                    && attribute.Size == attributeRequest.Size
                    && attribute.Color == attributeRequest.Color
                    && attribute.Brand == attributeRequest.Brand
                    );
            var product = new Product();
            var attribute = new Models.Attribute();
            if (existingProduct == null && existingAttribute == null)
            {


                product.Name = productRequest.Name;
                product.CatId = productRequest.CatId;

                await _context.AddAsync(product);
                await _context.SaveChangesAsync();

                attribute.Color = attributeRequest.Color;
                attribute.Brand = attributeRequest.Brand;
                attribute.Size = attributeRequest.Size;
                attribute.Type = attributeRequest.Type;
                await _context.AddAsync(attribute);
                await _context.SaveChangesAsync();
            }
            else if (existingProduct == null && existingAttribute != null)
            {
                product.Name = productRequest.Name;
                product.CatId = productRequest.CatId;
                await _context.AddAsync(product);
                await _context.SaveChangesAsync();

                attribute.Id = existingAttribute.Id;
            }
            else if (existingProduct != null && existingAttribute == null)
            {
                product.Id = existingProduct.Id;

                attribute.Color = attributeRequest.Color;
                attribute.Brand = attributeRequest.Brand;
                attribute.Size = attributeRequest.Size;
                attribute.Type = attributeRequest.Type;
                await _context.AddAsync(attribute);
                await _context.SaveChangesAsync();
            }
            else if (existingProduct != null && existingAttribute != null)
            {
                return BadRequest("Product & Attribute Already Exist!");
            }
            var intermediate = new Intermediate
            {
                ProductId = product.Id,
                AttributeId = attribute.Id,
                Price = price,
            };
            await _context.AddAsync(intermediate);
            await _context.SaveChangesAsync();
            return Ok();*/

        }

        // DELETE: api/Products/5
        [HttpDelete]
        [Route("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var getProductResponse = await productService.DeleteProdAsync(id);

            if (!getProductResponse.Success)
            {
                return BadRequest(new
                {
                    Success = false,
                    Error = "Not found",
                    ErrorCode = "S02"
                });
            }
            return Ok(getProductResponse);
            /* if (_context.Products == null)
             {
                 return NotFound();
             }
             var product = await _context.Products.FindAsync(id);
             if (product == null)
             {
                 return NotFound();
             }

             var DeleteInter = await _context.Intermediates.FirstOrDefaultAsync(x => x.ProductId == product.Id);
             if (DeleteInter != null)
             {
                 _context.Intermediates.Remove(DeleteInter);
                 await _context.SaveChangesAsync();
             }
             _context.Products.Remove(product);
             await _context.SaveChangesAsync();

             return Ok();*/
        }


    }
}
