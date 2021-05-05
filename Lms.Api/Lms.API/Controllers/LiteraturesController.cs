using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.API.Core.Entities;
using Lms.API.Data.Data;
using AutoMapper;
using Lms.API.Core.Repositories;

namespace Lms.API.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiteraturesController : ControllerBase
    {
        private readonly LmsAPIContext db;
        private readonly IMapper mapper;
        private readonly IUoW uow;

        public LiteraturesController(LmsAPIContext db, IMapper mapper, IUoW uow)
        {
            this.db = db;
            this.mapper = mapper;
            this.uow = uow;
        }

        // GET: api/Literatures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Literature>>> GetLiterature()
        {
            return Ok(await uow.LiteratureRepository.GetAllLiteratureAsync());
        }

        // GET: api/Literatures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Literature>> GetLiterature(int id)
        {
            var literature = await uow.LiteratureRepository.GetLiteratureAsync(id);

            if (literature == null)
            {
                return NotFound();
            }

            return Ok(literature);
        }

        [HttpGet("title/{title}")]
        public async Task<ActionResult<IEnumerable<Literature>>> GetLiteratureByTitle(string title)
        {
            return Ok(await uow.LiteratureRepository.GetLiteratureByTitleAsync(title));
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

            db.Entry(literature).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
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
            await uow.LiteratureRepository.AddAsync(literature);
            await uow.LiteratureRepository.SaveAsync();

            return CreatedAtAction("GetLiterature", new { id = literature.Id }, literature);
        }

        // DELETE: api/Literatures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLiterature(int id)
        {
            var literature = await uow.LiteratureRepository.GetLiteratureAsync(id);
            if (literature == null)
            {
                return NotFound();
            }

            uow.LiteratureRepository.Remove(literature);
            await uow.LiteratureRepository.SaveAsync();

            return NoContent();
        }

        private bool LiteratureExists(int id)
        {
            return db.Literature.Any(e => e.Id == id);
        }
    }
}
