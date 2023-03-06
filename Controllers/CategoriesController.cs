using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DenaAPI.Models;

namespace DenaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly DenaDbContext denaDbContext;

        public CategoriesController(DenaDbContext context)
        {
            denaDbContext = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            if (denaDbContext.Categories == null)
            {
                return NotFound();
            }
            return await denaDbContext.Categories.ToListAsync();
        }
        [HttpGet("{parentid},{isparent}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetChildren(int parentid, bool isparent = true)
        {
            if (denaDbContext.Categories == null)
            {
                return NotFound();
            }
            return await denaDbContext.Categories.Where(c => c.ParentId == parentid).ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            if (denaDbContext.Categories == null)
            {
                return NotFound();
            }
            var category = await denaDbContext.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            denaDbContext.Entry(category).State = EntityState.Modified;

            try
            {
                await denaDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            if (denaDbContext.Categories == null)
            {
                return Problem("Entity set 'DenaDbContext.Category'  is null.");
            }
            denaDbContext.Categories.Add(category);
            await denaDbContext.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (denaDbContext.Categories == null)
            {
                return NotFound();
            }
            var category = await denaDbContext.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            denaDbContext.Categories.Remove(category);
            await denaDbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool CategoryExists(int id)
        {
            return (denaDbContext.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
