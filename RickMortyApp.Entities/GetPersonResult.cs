using System.Collections.Generic;
using RickMortyApp.Entities.Person;

namespace RickMortyApp.Entities;

public class GetPersonResult
{
    public List<PersonResponse>? Persons { get; set; }

    public bool IsNotFound { get; set; } = false;
}
