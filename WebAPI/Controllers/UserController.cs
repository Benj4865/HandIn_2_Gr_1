using HandIn_2_Gr_1;
using HandIn_2_Gr_1.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebAPI.PostModels;

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

        //ReadAll Users
        [HttpGet("getusers/")]
        public IActionResult GetUsers(int page, int pageSize)
        {
            var userlist = DataService.GetUsers(page, pageSize);
            var pageObject = new Paging { users = userlist, nextpage = "/api/title/getusers?pagesize=" + pageSize.ToString() + "&page=" + (page + 1).ToString(), previouspage = "/api/title/getuserspagesize=" + pageSize.ToString() + "&page=" + (page - 1).ToString() };
            return Ok(pageObject);
        }
        //Create
        [HttpPost("createuser/")]
        public IActionResult CreateUser(UserBody data)
        {
            DataService.CreateUser(data.UserName, data.UserPassword, data.UserEmail);
            return Ok();
        }

        //Read
        [HttpGet("searchuser/")]
        public IActionResult SearchUser(string username="", string useremail ="", int userid=0)
        {
           var user = DataService.SearchUser(username, useremail, userid);

            return Ok(user);
        }

        //Read on UID
        [HttpGet("{userID}")]
        public IActionResult SearchUID(int userID)
        {
            var user = DataService.SearchUID(userID);

            return Ok(user);
        }
        
        //Update
        [HttpPost("updateuser/")]
        public IActionResult UpdateUser(UserBody data)
        {
            DataService.UpdateUser(data.UserID, data.UserName, data.UserPassword, data.UserEmail);
            return Ok();
        }

        //Delete
        [HttpPost("deleteuser/")]
        public IActionResult DeleteUser(UserBody data)
        {
            DataService.DeleteUser(data.UserID, data.UserPassword);
            return Ok();
        }

    }
}
