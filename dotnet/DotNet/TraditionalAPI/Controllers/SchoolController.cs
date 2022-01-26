using Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TraditionalAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ISchoolService schoolService;

        public SchoolController(ISchoolService schoolService)
        {
            this.schoolService = schoolService;
        }

        [HttpGet, Route("quick")]
        public IActionResult Quick()
        {
            return Ok("hello world");
        }

        [HttpGet, Route("enrollments")]
        public async Task<IActionResult> GetEnrollmentCount()
        {
            return Ok(await schoolService.GetEnrollmentCount());
        }

        [HttpGet, Route("enrollments/{start}/{count}")]
        public async Task<IActionResult> GetEnrollments(int start, int count)
        {
            try
            {
                var result = await schoolService.GetEnrollments(start, count);
                return Ok(result);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
