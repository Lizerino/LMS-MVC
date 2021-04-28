using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.API.Data.Data;
using Lms.API.Core.Entities;
using Lms.API.Core.Repositories;
using AutoMapper;
using Lms.API.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace Lms.API.UI.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
       // private readonly LmsAPIContext db;
        private readonly IUoW uoW;
        private readonly IMapper mapper;

        public CoursesController(LmsAPIContext db, IUoW uoW , IMapper mapper)
        {
          //  this.db = db;
            this.uoW = uoW;
            this.mapper = mapper;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDto>>> GetCoursesAsync(bool includeModules = false)
        {
            var courses = await uoW.CourseRepository.GetAllCoursesAsync(includeModules);

            var coursesDto = mapper.Map<IEnumerable<CourseDto>>(courses);
            return Ok(coursesDto);
        }
      

        // GET: api/Courses/5
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Route("{id:int}")]
        public async Task<ActionResult<CourseDto>> GetCourseAsync(int id)
        {

            var course = await uoW.CourseRepository.GetCourseAsync(id);

            if (course == null)
            {
                return NotFound();
            }
            var model = mapper.Map<CourseDto>(course);

            return Ok(model);
        } 
        
        //[HttpGet]
        //[Route("{id:string}")]
        //public async Task<ActionResult<CourseDto>> GetCourseAsync(string title)
        //{

        //    var course = await uoW.CourseRepository.GetCourseByTitleAsync(title);

        //    if (course == null)
        //    {
        //        return NotFound();
        //    }
        //    var model = mapper.Map<CourseDto>(course);

        //    return Ok(model);
        //}

       
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCourse(int id, CourseDto dto)
        {
            var course = await uoW.CourseRepository.GetCourseAsync(id);
            if (course is null)
            {
                return NotFound();
            }

            mapper.Map(dto, course);
            if (await uoW.CourseRepository.SaveAsync())
            {
                return Ok(mapper.Map<CourseDto>(course));
            }
            else
            {
                return StatusCode(500);
            }
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CourseDto>> CreateCourse(CourseDto dto)
        {

            var course = mapper.Map<Course>(dto);
            await uoW.CourseRepository.AddAsync(course);
            if ( await uoW.CourseRepository.SaveAsync())
            {
                var model = mapper.Map<CourseDto>(course);
                return CreatedAtAction("GetCourse", new { id = course.Id }, model);
                // The framework's default is to delet the Async from the action name.
                // the can be overridden in the startup class by adding an option " opt.SuppressAsyncSuffixInActionNames = false; " so the Add controller method will look like:
                /*
                 * services.AddControllers(opt =>
                        {
                            opt.ReturnHttpNotAcceptable = true;
                            opt.SuppressAsyncSuffixInActionNames = false;
                        })
                 * */
            }
            else
            {
                return StatusCode(500);
            }
        }
        



        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var course = await uoW.CourseRepository.GetCourseAsync(id);
            if (course is null)
            {
                return NotFound();
            }

            uoW.CourseRepository.Remove(course);

            await uoW.CourseRepository.SaveAsync();

            return NoContent();
        }
        
        [HttpPatch("{id:int}")]
        public async Task<ActionResult<CourseDto>> PatchCourse(int id, JsonPatchDocument<CourseDto> patchDocument)
        {
            var course = await uoW.CourseRepository.GetCourseAsync(id);
            if (course is null)
            {
                return NotFound();
            }
            var dto = mapper.Map<CourseDto>(course);
            patchDocument.ApplyTo(dto, ModelState);
            if (!TryValidateModel(dto))
                return BadRequest(ModelState);

            mapper.Map(dto, course);
            if (await uoW.CourseRepository.SaveAsync())
                return Ok(mapper.Map<CourseDto>(course));
            else
                return StatusCode(500);


        }



    }
}
