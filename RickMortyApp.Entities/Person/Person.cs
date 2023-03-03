namespace RickMortyApp.Entities.Person;

public class Person
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Status { get; set; }

    public string Species { get; set; }

    public string Type { get; set; }

    public PersonOrigin Origin { get; set; }
}
