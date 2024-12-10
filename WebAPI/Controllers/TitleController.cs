using HandIn_2_Gr_1;
using HandIn_2_Gr_1.Types;
using Microsoft.AspNetCore.Mvc;
using WebAPI.PostModels;

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

        //Create
        [HttpPost("createtitle/")]
        public IActionResult CreateTitle(TitleBody data)
        {
            var title = DataService.CreateTitle(data.Tconst, data.TitleType, data.PrimaryTitle, data.OriginalTitle, data.IsAdult, data.StartYear, data.EndYear, data.RuntimeMinutes, data.GenreList, data.PosterLink, data.plot);
            return Ok(title);
        }

        //Read
        [HttpGet("searchtitle/")]
        public IActionResult SearchTitleByName(string name)
        {
            var title = DataService.SearchTitleByName(name);
            return Ok(title);
        }

        //Update
        [HttpPost("updatetitle/")]
        public IActionResult updateTitle(TitleBody data)
        {
            var title = DataService.updateTitle(data.Tconst, data.TitleType, data.PrimaryTitle, data.OriginalTitle, data.IsAdult, data.StartYear, data.EndYear, data.RuntimeMinutes, data.GenreList, data.PosterLink, data.plot);
            return Ok(title);
        }
        
        //Delete



    }
}

