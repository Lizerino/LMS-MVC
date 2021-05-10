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
using Newtonsoft.Json;

namespace Lms.API.UI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicationsController : ControllerBase
    {
        private readonly LmsAPIContext db;
        private readonly IMapper mapper;
        private readonly IUoW uow;

        public PublicationsController(LmsAPIContext db, IMapper mapper, IUoW uow)
        {
            this.db = db;
            this.mapper = mapper;
            this.uow = uow;
        }

        // GET: api/Publications
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Publication>>> GetPublication()
        {
            return Ok(await uow.PublicationRepository.GetAllPublicationsAsync());
        }

        // GET: api/Publications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Publication>> GetPublication(int id)
        {
            var publicaton = await uow.PublicationRepository.GetPublicationAsync(id);

            if (publicaton == null)
            {
                return NotFound();
            }

            return Ok(publicaton);
        }

        [HttpGet("search/{search}")]
        public async Task<ActionResult<IEnumerable<Publication>>> GetPublicationBySearch(string search)
        {
            var result = await uow.PublicationRepository.GetPublicationBySearchAsync(search);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        // PUT: api/Publicatons/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPublication(int id, Publication publication)
        {
            if (id != publication.Id)
            {
                return BadRequest();
            }

            db.Entry(publication).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublicationExists(id))
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

        
        [HttpPost("create")]
        public async Task<ActionResult<Publication>> PostPublication([FromBody]string publication)
        {
            
            var newItem = JsonConvert.DeserializeObject<Publication>(publication);

            try
            {
                await uow.PublicationRepository.AddAsync(newItem);
                await uow.PublicationRepository.SaveAsync();

                return CreatedAtAction(nameof(GetPublication), new { id = newItem.Id }, newItem);//TODO Make a better response for Details view or something
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        // DELETE: api/Publications/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePublication(int id)
        {
            var publication = await uow.PublicationRepository.GetPublicationAsync(id);
            if (publication == null)
            {
                return NotFound();
            }

            uow.PublicationRepository.Remove(publication);
            await uow.PublicationRepository.SaveAsync();

            return NoContent();
        }

        private bool PublicationExists(int id)
        {
            return db.Publications.Any(e => e.Id == id);
        }
    }
}
