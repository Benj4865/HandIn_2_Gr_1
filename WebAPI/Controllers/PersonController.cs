using HandIn_2_Gr_1;
using HandIn_2_Gr_1.Types;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult SearchByName(string name)
        {
            var person = DataService.SearchByName(name);
            return Ok(person);
        }


        //HTTP create funtions
        [HttpPost("createperson")]
        public IActionResult createPerson([FromBody] Person newPerson)
        {
            try
            {
                var createdPerson = DataService.createPerson(newPerson);
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


        [HttpPost("updateperson/")]
        public IActionResult UpdatePerson(PersonBody data)
        {
            var person = DataService.updatePerson(data.Nconst, data.Primaryname, data.Birthyear, data.Deathyear, data.Primaryprofessions, data.KnownFor);
            return Ok(person);
        }


        [HttpGet("profession/{profession}")]
        public IActionResult SearchByProfession(string profession)
        {
            var persons = DataService.SearchByProfession(profession);
            return Ok(persons);
        }

        [HttpGet("knownfor/{NConst}")]
        public IActionResult FindKnownForTitles(string NConst)
        {
            var persons = DataService.FindKnownForTitles(NConst);
            return Ok(persons);
        }

    }

    
}
