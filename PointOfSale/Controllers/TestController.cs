using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PointOfSale.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController()
        {
            
        }
        [Authorize(Policy = "CookiePolicy")]

        [HttpGet("GetByCookie")]
        public IActionResult GetByCookie()
        {
            return Ok("you hit by cookie");
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "JwtPolicy")]
        [HttpGet("GetByJWT")]

        public IActionResult GetByJWT()
        {
            return Ok("you hit by jwt");
        }

        [Authorize]
        [HttpGet("GetByBoth")]

        public IActionResult GetByBoth()
        {
            return Ok("you hit by both jwt and cookie");
        }
    }
}
