using HandIn_2_Gr_1;
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
        /*
        [HttpGet("knownfor/{NConst}")]
        public IActionResult FindKnownForTitles(string NConst)
        {
            var persons = DataService.FindKnownForTitles(NConst);
            return Ok(persons);
        }
        */
    }
}

