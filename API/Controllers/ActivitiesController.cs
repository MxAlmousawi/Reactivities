using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        private readonly DataContext context;

        public ActivitiesController(DataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Activity>>> GetActivities()
        {
            return await context.Activities.ToListAsync();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Activity>> GetActivities([FromRoute]Guid id)
        {
            return await context.Activities.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
