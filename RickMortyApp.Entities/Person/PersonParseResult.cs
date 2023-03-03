using System.Collections.Generic;

namespace RickMortyApp.Entities.Person;

public class PersonParseResult
{
    public List<Person> Persons { get; set; }

    public List<PersonEpisodes> PersonEpisodes { get; set; }
}
