using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.MVC.Data.Data;
using Lms.API.Data.Data;
using Lms.API.Core.Entities;
using Lms.API.Core.Repositories;
using AutoMapper;
using Lms.API.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace Lms.API.UI.Controllers
{
    [Route("api/courses/{id}/modules")]
    [ApiController]
    public class ModulesController : ControllerBase
    {

        private readonly IUoW uoW;
        private readonly IMapper mapper;

        public ModulesController( IUoW uoW, IMapper mapper)
        {
            this.uoW = uoW;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<ModuleDto>> GetAllModulesAsync(int id)
        {
            if (await uoW.CourseRepository.GetCourseAsync(id) is null)
            {
                return BadRequest();
            }

            var modules = await uoW.ModuleRepository.GetAllModulesAsync(id);
            if (!(modules.Any()))
            {
                return NoContent();
            }
            var model = mapper.Map<IEnumerable<ModuleDto>>(modules);
            return Ok(model);
        }

        [HttpGet("{moduleId:int}")]
        public async Task<ActionResult<ModuleDto>> GetModuleAsync(int id, int moduleId)
        {
            if (await uoW.CourseRepository.GetCourseAsync(id) is null) return BadRequest();


            var module = await uoW.ModuleRepository.GetModuleAsync(id, moduleId);
            if (module is null)
            {
                return NotFound();
            }
            var model = mapper.Map<ModuleDto>(module);
            return Ok(model);
        }


        [HttpGet("{title}")]
        public async Task<ActionResult<ModuleDto>> GetModuleByTitleAsync(int id, string title)
        {
            if (await uoW.CourseRepository.GetCourseAsync(id) is null) return BadRequest();


            var module = await uoW.ModuleRepository.GetModuleByTitleAsync(id, title);
            if (module is null)
            {
                return NotFound();
            }
            var model = mapper.Map<ModuleDto>(module);
            return Ok(model);
        }


     //   [HttpPost]
        //[Route("CreateModule")]
        //public async Task<ActionResult<ModuleDto>> CreateModule(int id, ModuleDto dto)
        //{
        //    var course = await uoW.CourseRepository.GetCourseAsync(id);
        //    if (course is null)
        //    {
        //        return BadRequest();
        //    }
        //    var module = mapper.Map<Module>(dto);
        //    module.CourseId = id;
        //    await uoW.ModuleRepository.AddAsync(module);
        //    if (await uoW.ModuleRepository.SaveAsync())
        //    {
        //        var model = mapper.Map<ModuleDto>(module);
        //        return CreatedAtAction("GetModule", new { id = module.CourseId, moduleId = module.Id }, model);
        //    }
        //    else
        //    {
        //        return StatusCode(500);
        //    }
        //}


        [HttpPost(Name = "CreateModule")]
        [Route("/create")]
        public async Task<ActionResult<ModuleDto>> CreateModule(int id, ModuleDto dto)
        {
            //Find Course
            var course = await uoW.CourseRepository.GetCourseAsync(id);
            if (course is null)
            {
                return BadRequest();
            }
            
            //Map Module and add to database
            var module = mapper.Map<Module>(dto);
            module.CourseId = id;
            await uoW.ModuleRepository.AddAsync(module);
            if (await uoW.ModuleRepository.SaveAsync())
            {
                var model = mapper.Map<ModuleDto>(module);
                return CreatedAtAction("GetModuleByTitleAsync", new { id = module.CourseId, title = module.Title }, model);
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPut("{title}")]
        public async Task<ActionResult<ModuleDto>> PutModuleAsync(int id, string title, ModuleDto dto)
        {
            var course = await uoW.CourseRepository.GetCourseAsync(id);
            if (course is null)
            {
                return BadRequest();
            }
            var module = await uoW.ModuleRepository.GetModuleByTitleAsync(id, title);
            if (module is null)
            {
                return NotFound();
            }
            mapper.Map(dto, module);
            if (await uoW.ModuleRepository.SaveAsync())
            {
                return Ok(mapper.Map<ModuleDto>(module));
            }
            else
            {
                return StatusCode(500);
            }

        }
        [HttpPatch("{moduleId:int}")]
        public async Task<ActionResult<ModuleDto>> PatchModuleAsync(int id, int moduleId, JsonPatchDocument<ModuleDto> patchDocument)
        {
            var course = await uoW.CourseRepository.GetCourseAsync(id);
            if (course is null)
            {
                return BadRequest();
            }
            var module = await uoW.ModuleRepository.GetModuleAsync(id, moduleId);
            if (module is null)
            {
                return NotFound();
            }
            var dto = mapper.Map<ModuleDto>(module);
            patchDocument.ApplyTo(dto, ModelState);
            if (!TryValidateModel(dto))
                return BadRequest(ModelState);
            mapper.Map(dto, module);
            if (await uoW.ModuleRepository.SaveAsync())
            {
                return Ok(mapper.Map<ModuleDto>(module));
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpPatch("{title}")]
        public async Task<ActionResult<ModuleDto>> PatchModuleByTitleAsync(int id, string title, JsonPatchDocument<ModuleDto> patchDocument)
        {
            var course = await uoW.CourseRepository.GetCourseAsync(id);
            if (course is null)
            {
                return BadRequest();
            }
            var module = await uoW.ModuleRepository.GetModuleByTitleAsync(id, title);
            if (module is null)
            {
                return NotFound();
            }
            var dto = mapper.Map<ModuleDto>(module);
            patchDocument.ApplyTo(dto, ModelState);
            if (!TryValidateModel(dto))
                return BadRequest(ModelState);
            mapper.Map(dto, module);
            if (await uoW.ModuleRepository.SaveAsync())
            {
                return Ok(mapper.Map<ModuleDto>(module));
            }
            else
            {
                return StatusCode(500);
            }
        }

        [HttpDelete("{moduleId:int}")]
        public async Task<IActionResult> DeleteModule(int id, int moduleId)
        {
            var course = await uoW.CourseRepository.GetCourseAsync(id);
            if (course is null)
            {
                return BadRequest();
            }
            var module = await uoW.ModuleRepository.GetModuleAsync(id, moduleId);
            if (module is null)
            {
                return NotFound();
            }
            uoW.ModuleRepository.Remove(module);

            await uoW.ModuleRepository.SaveAsync();


            return NoContent();
        }
        [HttpDelete("{title}")]
        public async Task<IActionResult> DeleteModuleByTitle(int id, string title)
        {
            var course = await uoW.CourseRepository.GetCourseAsync(id);
            if (course is null)
            {
                return BadRequest();
            }
            var module = await uoW.ModuleRepository.GetModuleByTitleAsync(id, title);
            if (module is null)
            {
                return NotFound();
            }
            uoW.ModuleRepository.Remove(module);

            await uoW.ModuleRepository.SaveAsync();


            return NoContent();
        }
    }
}
