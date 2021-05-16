using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using Lms.MVC.UI.Models.ViewModels;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

namespace Lms.MVC.UI.Controllers
{
    [Route("Controllers/[controller]")]
    [ApiController]
    public class EventsController : Controller
    {
        private readonly IUoW uow;
        private readonly IMapper mapper;

        public EventsController(IUoW uow, IMapper mapper)
        {
            this.uow     = uow;
            this.mapper = mapper;
        }

        // GET api/events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> GetEvents(int Id)
        {
            var activities = await uow.ActivityRepository.GetAllActivitiesByModuleIdAsync(Id);
            var events = new List<SchedulerEvent>();
            foreach (var act in activities)
            {
                var calevent = new SchedulerEvent();
                calevent.Id = act.Id;
                calevent.Title = act.Title;
                calevent.text = act.Description;
                calevent.start_date = act.StartDate;
                calevent.end_date = act.EndDate;
                events.Add(calevent);
            }
            var response = JsonConvert.SerializeObject(events);
            return Ok(response);
        }

        // GET api/events/5
        [HttpGet("{id}")]
        public SchedulerEvent Get(int id)
        {
            var activity = uow.ActivityRepository.GetActivityAsync(id).Result;
            var result = mapper.Map<SchedulerEvent>(activity);
            return result;
        }
    }
}