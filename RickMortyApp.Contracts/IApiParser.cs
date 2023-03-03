using System.Threading.Tasks;
using RickMortyApp.Entities.Episode;
using RickMortyApp.Entities.Person;

namespace RickMortyApp.Contracts;

public interface IApiParser
{
    Task<PersonParseResult> ParsePersonsAsync(string name);

    Task<EpisodeParseResult> ParseEpisodesAsync(string name);
}
