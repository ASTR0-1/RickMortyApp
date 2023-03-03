using Microsoft.AspNetCore.Mvc;
using RickMortyApp.Contracts;
using RickMortyApp.Entities.Person;

namespace RickMortyApp.Api.Controllers;

[ApiController]
[Route("/api/v1")]
public class PersonController : ControllerBase
{
    private readonly IPersonService _personService;
    
    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [HttpPost("check-person")]
    public async Task<ActionResult<bool>> PostCheckPerson([FromBody] CheckPersonRequest request)
    {
        var res = await _personService.CheckPersonInEpisode(request.PersonName, request.EpisodeName);

        if (res.IsNotFound)
            return NotFound();

        return Ok(res.IsInEpisode);
    }

    [HttpGet("person")]
    public async Task<ActionResult<bool>> GetPerson([FromQuery] string name)
    {
        var res = await _personService.GetPersonByName(name);

        if (res.IsNotFound)
            return NotFound();

        return Ok(res.Persons);
    }
}
