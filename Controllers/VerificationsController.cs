using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DenaAPI;
using DenaAPI.Models;

namespace DenaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VerificationsController : ControllerBase
    {
        private readonly DenaDbContext _context;

        public VerificationsController(DenaDbContext context)
        {
            _context = context;
        }

        // GET: api/Verifications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Verification>>> GetVerifications()
        {
            if (_context.Verifications == null)
            {
                return NotFound();
            }
            return await _context.Verifications.ToListAsync();
        }

        // GET: api/Verifications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Verification>> GetVerification(int id)
        {
            if (_context.Verifications == null)
            {
                return NotFound();
            }
            var verification = await _context.Verifications.FindAsync(id);

            if (verification == null)
            {
                return NotFound();
            }

            return verification;
        }

        // PUT: api/Verifications/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVerification(int id, Verification verification)
        {
            if (id != verification.Id)
            {
                return BadRequest();
            }

            _context.Entry(verification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VerificationExists(id))
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

        // POST: api/Verifications
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Verification>> PostVerification([FromForm] Verification verification)
        {
            if (_context.Verifications == null)
            {
                return Problem("Entity set 'DenaDbContext.Verifications'  is null.");
            }
            _context.Verifications.Add(verification);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Verifications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVerification(int id)
        {
            if (_context.Verifications == null)
            {
                return NotFound();
            }
            var verification = await _context.Verifications.FindAsync(id);
            if (verification == null)
            {
                return NotFound();
            }

            _context.Verifications.Remove(verification);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VerificationExists(int id)
        {
            return (_context.Verifications?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
