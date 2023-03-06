using DenaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DenaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Users1Controller : BaseApiController
    {
        private readonly DenaDbContext denaDbContext;
        public Users1Controller(DenaDbContext context)
        {
            denaDbContext = context;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            denaDbContext.Entry(user).State = EntityState.Modified;

            try
            {
                await denaDbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        private bool UserExists(int id)
        {
            return (denaDbContext.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
