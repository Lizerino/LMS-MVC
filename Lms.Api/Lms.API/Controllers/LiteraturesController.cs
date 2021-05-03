using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.API.Core.Entities;
using Lms.API.Data.Data;

namespace Lms.API.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiteraturesController : ControllerBase
    {
        private readonly LmsAPIContext _context;

        public LiteraturesController(LmsAPIContext context)
        {
            _context = context;
        }

        // GET: api/Literatures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Literature>>> GetLiterature()
        {
            return await _context.Literature.ToListAsync();
        }

        // GET: api/Literatures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Literature>> GetLiterature(int id)
        {
            var literature = await _context.Literature.FindAsync(id);

            if (literature == null)
            {
                return NotFound();
            }

            return literature;
        }

        // PUT: api/Literatures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLiterature(int id, Literature literature)
        {
            if (id != literature.Id)
            {
                return BadRequest();
            }

            _context.Entry(literature).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiteratureExists(id))
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

        // POST: api/Literatures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Literature>> PostLiterature(Literature literature)
        {
            _context.Literature.Add(literature);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLiterature", new { id = literature.Id }, literature);
        }

        // DELETE: api/Literatures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLiterature(int id)
        {
            var literature = await _context.Literature.FindAsync(id);
            if (literature == null)
            {
                return NotFound();
            }

            _context.Literature.Remove(literature);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LiteratureExists(int id)
        {
            return _context.Literature.Any(e => e.Id == id);
        }
    }
}
