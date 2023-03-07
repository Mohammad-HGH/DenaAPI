using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DenaAPI.DTO;

namespace DenaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttributesController : ControllerBase
    {
        private readonly DenaDbContext _context;

        public AttributesController(DenaDbContext context)
        {
            _context = context;
        }

        // GET: api/Attributes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Attribute>>> GetAttributes()
        {
            if (_context.Attributes == null)
            {
                return NotFound();
            }
            return await _context.Attributes.ToListAsync();
        }

        // GET: api/Attributes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Attribute>> GetAttribute(int id)
        {
            if (_context.Attributes == null)
            {
                return NotFound();
            }
            var attribute = await _context.Attributes.FindAsync(id);

            if (attribute == null)
            {
                return NotFound();
            }

            return attribute;
        }

        // PUT: api/Attributes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAttribute(int id, AttributeRequest attribute)
        {
            var attribute1 = await _context.Attributes.FirstOrDefaultAsync(x => x.Id == id);

            if (attribute1 == null)
            {
                return BadRequest("User Not Found!");
            }

            attribute1.Brand = attribute.Brand;
            attribute1.Color = attribute.Color;
            attribute1.Size = attribute.Size;
            attribute1.Type = attribute.Type;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttributeExists(id))
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

        // POST: api/Attributes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Models.Attribute>> PostAttribute([FromForm] AttributeRequest attribute)
        {
            var Att = await _context.Attributes.SingleOrDefaultAsync(x => x.Type == attribute.Type &&
                                                                          x.Size == attribute.Size &&
                                                                          x.Color == attribute.Color &&
                                                                          x.Brand == attribute.Brand);
            if (Att == null)
            {
                Models.Attribute attribute1 = new()
                {
                    Brand = attribute.Brand,
                    Color = attribute.Color,
                    Size = attribute.Size,
                    Type = attribute.Type
                };
                _context.Attributes.Add(attribute1);
                await _context.SaveChangesAsync();
            }
            else
                return BadRequest("Attribute is Exist!");
            return Ok();
        }

        // DELETE: api/Attributes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttribute(int id)
        {
            if (_context.Attributes == null)
            {
                return NotFound();
            }
            var attribute = await _context.Attributes.FindAsync(id);
            if (attribute == null)
            {
                return NotFound();
            }

            _context.Attributes.Remove(attribute);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool AttributeExists(int id)
        {
            return (_context.Attributes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
