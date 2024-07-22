using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StudentInformationSystem.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        [HttpPost]
        [Route("CreateRecurringJob")]
        public ActionResult CreateRecurringJob()
        {
            RecurringJob.AddOrUpdate(
                "CreateRecurringJob",
                () => Console.WriteLine("Recurring!"),
                Cron.Minutely);

            return Ok();
        }
    }
}
