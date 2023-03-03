using System.Collections.Generic;

namespace RickMortyApp.Entities.Person;

public class PersonEpisodes
{
    public int PersonId { get; set; }

    public IEnumerable<int> EpisodeIds { get; set; }
}