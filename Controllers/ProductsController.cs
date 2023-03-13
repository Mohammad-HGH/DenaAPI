using Microsoft.AspNetCore.Mvc;
using DenaAPI.Models;
using DenaAPI.DTO;
using DenaAPI.Interfaces;

namespace DenaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : BaseApiController
    {
        private readonly IProductService productService;

        public ProductsController([FromForm] IProductService productService)
        {
            this.productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        [Route("ProductsList")]
        public async Task<IActionResult> GetProducts()
        {
            var getProductResponse = await productService.GetAllProdsAsync();

            if (!getProductResponse.Success)
            {
                return BadRequest(getProductResponse);
            }
            return Ok(getProductResponse);
        }

        // GET: api/Products/5
        [HttpGet]
        [Route("ViewProductById")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var getProductResponse = await productService.GetProdAsync(id);

            if (!getProductResponse.Success)
            {
                return BadRequest(getProductResponse);
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
                return BadRequest(getProductResponse);
            }
            return Ok(getProductResponse);


        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult> PostProduct([FromForm] ProductRequest productRequest, [FromForm] AttributeRequest attributeRequest, [FromForm] long price)
        {
            var getProductResponse = await productService.CreateProdAsync(productRequest, attributeRequest, price);

            if (!getProductResponse.Success)
            {
                return BadRequest(getProductResponse);
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
                return BadRequest(getProductResponse);
            }
            return Ok(getProductResponse);

        }


    }
}
