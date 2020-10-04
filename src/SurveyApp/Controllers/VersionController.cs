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
#pragma warning disable SEC0120
        public IActionResult Get()
#pragma warning restore SEC0120
        {
            return Ok(new Version(1, 0, 0));
        }
    }
}