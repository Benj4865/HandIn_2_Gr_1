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

        [HttpGet("getuser/")]
        public IActionResult GetUsers()
        {
            var users = DataService.GetUsers();
            return Ok(users);
        }

        [HttpGet("createuser/{Type Ok, to confirm creation}")]
        public IActionResult CreateUser(int userID, string username, string password, string email)
        {
            DataService.CreateUser(userID, username, password, email);
            return Ok();
        }


        [HttpGet("deleteuser/{Type Ok, to confirm deletion}")]
        public IActionResult DeleteUser(int userID, string password)
        {
            DataService.DeleteUser(userID, password);
            return Ok();
        }


    }
}
