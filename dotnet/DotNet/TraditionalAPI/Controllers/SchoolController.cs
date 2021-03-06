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
            var result = await schoolService.GetEnrollments(start, count);
            return Ok(result);
        }

        [HttpPost, Route("enrollmentsSplit")]
        public async Task<IActionResult> GetEnrollmentsSplit([FromBody] List<long> enrollmentIds)
        {
            //enrollmentIds = Enumerable.Range(10002, 1000).Select(x=> (long)x).ToList();
            var result = await schoolService.GetEnrollmentsSplit(enrollmentIds);
            return Ok(result);
        }

        private const int MaxPagingSize = 500;

        [HttpPost, Route("v1")]
        public async Task<IActionResult> GetEnrollmentsV1([FromBody] PagingViewModel paging)
        {
            if(paging == null || paging.Count > MaxPagingSize)
            {
                return BadRequest();
            }

            var result = await schoolService.GetEnrollmentsV1(paging);
            return Ok(result);
        }

        [HttpPost, Route("v2")]
        public async Task<IActionResult> GetEnrollmentsV2([FromBody] PagingViewModel paging)
        {
            if (paging == null || paging.Count > MaxPagingSize)
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(paging.OrderBy))
            {
                paging.OrderBy = "enrollmentid";
            }

            try
            {
                var result = await schoolService.GetEnrollmentsV2(paging);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

    }
}
