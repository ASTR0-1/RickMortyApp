namespace RickMortyApp.Entities.Person;

public class PersonResponse
{
    public string Name { get; set; }

    public string Status { get; set; }

    public string Species { get; set; }

    public string Type { get; set; }

    public PersonOrigin Origin { get; set; }
}
