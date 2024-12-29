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

        //Read by name
        // If a value for pagesize is passed wth the function, it will be used. Otherwies it will be 50
        [HttpGet("searchtitlebyname/")]
        public IActionResult SearchTitleByName(string name, int pagesize= 50, int page= 1)
        {
            // If people use too big or small a pagesize, we will revert it to 50
            if (pagesize > 50 || pagesize <= 0)
            {
                pagesize = 50;
            }

            // To make sure no-one is searching for page -1
            if (page <= 0)
            {
                page = 1;
            }

            var titleList = DataService.SearchTitleByName(name, pagesize, page);
            var pageObject = new Paging{ titles = titleList, nextpage = "/api/title/searchtitlebyname?name=e&pagesize=10&page=" + (page + 1).ToString(), previouspage = "/api/title/searchtitlebyname?name=e&pagesize=10&page=" + (page - 1).ToString()};
            return Ok(pageObject);
        }


        // api/title/{tconst}
        [HttpGet("{tconst}")]
        public IActionResult SearchTitleByTConst(string tconst)
        {
            var title = DataService.SearchTitleByTConst(tconst);
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
        [HttpPost("deletetitle/")]
        public IActionResult deleteTitle(TitleBody data)
        {
            DataService.DeleteTitle(data .Tconst);
            return Ok();
        }

        // Here we try to handle badrequest
        [HttpGet("bookmarktitle/{linkstring}")]
        public IActionResult bookmarkTitle(string linkstring)
        {
            if (DataService.bookmarkTitle(linkstring))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}

