using MarauderServer.Models;
using MarauderServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarauderServer.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class API_StatisticsController : ControllerBase
    {
        private StatisticsService statisticsService;

        public API_StatisticsController(StatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        // GET: api/Statistics
        // [HttpGet]
        public ActionResult<Statistics> Get()
        {
            Statistics? obj = statisticsService.GetStatistics();
            if (obj == null)
            {
                return NotFound();
            }
            return Ok(obj);
        }
    }
}
