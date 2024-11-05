using HandIn_2_Gr_1;
using HandIn_2_Gr_1.Types;
using Microsoft.AspNetCore.Mvc;
//using WebAPI.Models;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        IDataServiceUser DataService;
        private readonly LinkGenerator _linkGenerator;

        public UserController(
        IDataServiceUser dataService,
        LinkGenerator linkGenerator)
        {
            DataService = dataService;
            _linkGenerator = linkGenerator;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            var users = DataService.GetUsers();
            return Ok(users);
        }

    }
}