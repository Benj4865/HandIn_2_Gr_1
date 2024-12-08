﻿using HandIn_2_Gr_1;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("getperson/{nconst}")]
        public IActionResult GetPerson(string nconst)
        {
            var person = DataService.GetPerson(nconst);
            return Ok(person);
        }

        [HttpGet("getpersonname/{name}")]
        public IActionResult SearchByName(string name)
        {
            var person = DataService.SearchByName(name);
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
