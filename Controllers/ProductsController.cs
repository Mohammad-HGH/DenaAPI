using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DenaAPI.Models;
using DenaAPI.DTO;

namespace DenaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DenaDbContext _context;

        public ProductsController(DenaDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            if (_context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductRequest product)
        {
            var product1 = await _context.Products.FindAsync(id);
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

            return Ok();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromForm] ProductRequest productRequest, [FromForm] AttributeRequest attributeRequest, [FromForm] long price)
        {
            var existingProduct = await _context.Products.SingleOrDefaultAsync(
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
            return Ok();

        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Products == null)
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

            return Ok();
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
