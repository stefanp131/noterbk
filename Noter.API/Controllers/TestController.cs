using Microsoft.AspNetCore.Mvc;
using Noter.DAL.Context;

namespace Noter.API.Controllers
{
    [Route("api/test")]
    public class TestController : Controller
    {
        private readonly NoterContext context;

        public TestController(NoterContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Test()
        {
            return Ok();
        }
    }
}
