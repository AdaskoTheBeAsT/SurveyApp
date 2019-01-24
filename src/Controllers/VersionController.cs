using System;
using Microsoft.AspNetCore.Mvc;

namespace SurveyApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(Version), 200)]
        public IActionResult Get()
        {
            return Ok(new Version(1, 0, 0));
        }
    }
}