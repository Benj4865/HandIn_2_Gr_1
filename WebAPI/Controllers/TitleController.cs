using HandIn_2_Gr_1;
using HandIn_2_Gr_1.Types;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/title")]
    public class TitleController : ControllerBase
    {
        IDataServiceTitle DataService;
        private readonly LinkGenerator _linkGenerator;

        public TitleController(
        IDataServiceTitle dataService,
        LinkGenerator linkGenerator)
        {
            DataService = dataService;
            _linkGenerator = linkGenerator;
        }
        
        [HttpGet("searchtitle/")]
        public IActionResult SearchTitleByName(string name)
        {
            var title = DataService.SearchTitleByName(name);
            return Ok(title);
        }
        
    }
}

