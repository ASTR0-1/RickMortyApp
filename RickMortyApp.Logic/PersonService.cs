using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RickMortyApp.Contracts;
using RickMortyApp.Entities;
using RickMortyApp.Entities.Person;

namespace RickMortyApp.Logic;

public class PersonService : IPersonService
{
    private readonly IApiParser _apiParser;

    public PersonService(IApiParser apiParser)
    {
        _apiParser = apiParser;
    }

    public async Task<CheckPersonInEpisodeResult> CheckPersonInEpisode(string personName, string episodeName)
    {
        var personsParseResult = await _apiParser.ParsePersonsAsync(personName);
        var episodesParseResult = await _apiParser.ParseEpisodesAsync(episodeName);

        if (personsParseResult?.Persons == null
                || personsParseResult?.Persons?.Count == 0
                || episodesParseResult?.Episodes == null)
            return new CheckPersonInEpisodeResult
            {
                IsInEpisode = null,
                IsNotFound = true
            };

        bool isInEpisode = true;

        foreach (var person in personsParseResult?.Persons)
        {
            foreach (var episode in episodesParseResult.Episodes)
            {
                var personEpisodes = personsParseResult.PersonEpisodes
                        .Where(p => p.PersonId == person?.Id)
                        .SingleOrDefault();

                if (personEpisodes.EpisodeIds.Contains(episode.Id))
                    isInEpisode = true;
            }
        }

        return new CheckPersonInEpisodeResult
        {
            IsInEpisode = isInEpisode,
        };
    }

    public async Task<GetPersonResult> GetPersonByName(string personName)
    {
        var personsParseResult = await _apiParser.ParsePersonsAsync(personName);

        if (personsParseResult == null 
            || personsParseResult.Persons == null)
            return new GetPersonResult
            {
                Persons = null,
                IsNotFound = true
            };

        List<PersonResponse> personResponse = new List<PersonResponse>();

        foreach (var person in personsParseResult.Persons)
        {
            personResponse.Add(new PersonResponse
            {
                Name = person.Name,
                Status = person.Status,
                Species = person.Species,
                Type = person.Type,
                Origin = person.Origin,
            });
        }

        return new GetPersonResult
        {
            Persons = personResponse,
        };
    }
}
