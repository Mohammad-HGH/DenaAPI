using Microsoft.AspNetCore.Mvc;
using DenaAPI.Models;
using DenaAPI.DTO;
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
            
        }


    }
}
