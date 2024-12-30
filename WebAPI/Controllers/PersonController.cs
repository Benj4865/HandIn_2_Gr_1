using HandIn_2_Gr_1;
using HandIn_2_Gr_1.Types;
using Microsoft.AspNetCore.Mvc;
using System;
using WebAPI.PostModels;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/person")]
    public class PersonController : ControllerBase
    {

        IDataServicePerson DataService;
        private readonly LinkGenerator _linkGenerator;

        public PersonController(
        IDataServicePerson dataService,
        LinkGenerator linkGenerator)
        {
            DataService = dataService;
            _linkGenerator = linkGenerator;
        }

        //HTPP person functions
        //HTTP get functions
        [HttpGet("{nconst}")]
        public IActionResult GetPerson(string nconst)
        {
            var person = DataService.GetPerson(nconst);
            return Ok(person);
        }

        [HttpGet("searchbyname/{name}")]
        public IActionResult SearchByName(string name, int pagesize, int page)
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

            var personList = DataService.SearchByName(name, pagesize, page);
            var pageObejct = new Paging { people = personList, nextpage = "/api/person/searchbyname/" + name + "?pagesize=" + pagesize.ToString() + "&page=" + (page + 1).ToString(), previouspage = "/api/person/searchbyname/" + name + "?pagesize=" + pagesize.ToString() + "&page=" + (page - 1).ToString() };
            return Ok(pageObejct);
        }


        //HTTP create functions
        //We also try to implement errorhandling if the creation of the person, fails
        [HttpPost("createperson/")]
        public IActionResult createPerson(string Primaryname, string Birthyear, string Deathyear)
        {
            try
            {
                Person newperson = new Person
                {
                    Primaryname = Primaryname,
                    Birthyear = Birthyear,
                    Deathyear = Deathyear
                };

                var createdPerson = DataService.createPerson(newperson);
                if (createdPerson != null)
                {
                    return Ok(new
                    {
                        message = "Person succesfully created."
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        message = "Failed to create person"
                    });
                }
            }
            catch (Exception ex)
            {

                return StatusCode(500, new
                {
                    message = "An internal server error occured.",
                    error = ex.Message
                });
            }
        }
        // HTTP Update functions

        //Here we split up the input to the function, instead of using the Person-Body
        //This is due to the way we send data from the fronten. It is not the prettiest solution
        //, but it was the only way we could make it work
        [HttpPost("updateperson/")]
        public IActionResult UpdatePerson(string Nconst, string Primaryname, string Birthyear, string Deathyear,  string UpdatePrimaryprofession, string UpdateKnownFor)
        {
            var person = DataService.updatePerson(Nconst, Primaryname, Birthyear, Deathyear, UpdatePrimaryprofession, UpdateKnownFor);
            return Ok(person);
        }


        [HttpGet("profession/{profession}")]
        public IActionResult SearchByProfession(string profession, int pagesize = 50, int page = 1)
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

            var professionList = DataService.SearchByProfession(profession, pagesize, page);
            var pageObject = new Paging { people = professionList, nextpage = "/api/person/profession/" + profession + "?pagesize=" + pagesize.ToString() + "&page=" + (page + 1).ToString(), previouspage = "/api/person/profession/" + profession + "?pagesize=" + pagesize.ToString() + "&page=" + (page - 1).ToString() };
            return Ok(pageObject);
        }

        // Here we try to handle badrequest
        [HttpGet("bookmarkperson/{linkstring}")]
        public IActionResult bookmarkPerson(string linkstring)
        {
            if (DataService.bookmarkPerson(linkstring))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
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
