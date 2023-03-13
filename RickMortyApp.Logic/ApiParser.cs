using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Caching.Memory;
using RickMortyApp.Contracts;
using RickMortyApp.Entities.Episode;
using RickMortyApp.Entities.Person;

namespace RickMortyApp.Logic;

public class ApiParser : IApiParser
{
    public readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _memoryCache;

    public ApiParser(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
    {
        _httpClientFactory = httpClientFactory;
        _memoryCache = memoryCache;
    }

    public async Task<EpisodeParseResult> ParseEpisodesAsync(string name)
    {
        var episodes = new List<Episode>();
        string apiUrl = $"https://rickandmortyapi.com/api/episode/?name={HttpUtility.UrlEncode(name)}";

        var httpClient = _httpClientFactory.CreateClient();

        while (!string.IsNullOrEmpty(apiUrl))
        {
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl).ConfigureAwait(false);
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            JsonDocument json = JsonDocument.Parse(content);

            var res = json.RootElement.TryGetProperty("results", out JsonElement jsonElement);

            if (res == false)
                return new EpisodeParseResult
                {
                    Episodes = null
                };

            var results = jsonElement.EnumerateArray();

            foreach (var result in results)
            {
                var episode = new Episode
                {
                    Id = result.GetProperty("id").GetInt32(),
                    Name = result.GetProperty("name").GetString()
                };

                episodes.Add(episode);
            }

            apiUrl = json.RootElement.GetProperty("info")
                .GetProperty("next")
                .GetString();

            apiUrl = !string.IsNullOrEmpty(apiUrl) ? apiUrl + $"&name={HttpUtility.UrlPathEncode(name)}" : apiUrl;
        }

        _memoryCache.Set("EpisodesCacheKey", episodes, DateTimeOffset.UtcNow.AddMinutes(1));

        return new EpisodeParseResult
        {
            Episodes = episodes
        };
    }

    public async Task<PersonParseResult> ParsePersonsAsync(string name)
    {
        var characters = new List<Person>();
        var personEpisodesList = new List<PersonEpisodes>();
        string apiUrl = $"https://rickandmortyapi.com/api/character/?name={HttpUtility.UrlPathEncode(name)}";

        var httpClient = _httpClientFactory.CreateClient();

        while (!string.IsNullOrEmpty(apiUrl))
        {
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl).ConfigureAwait(false);
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            JsonDocument json = JsonDocument.Parse(content);

            var res = json.RootElement.TryGetProperty("results", out JsonElement jsonElement);

            if (res == false)
                return new PersonParseResult
                {
                    PersonEpisodes = null,
                    Persons = null,
                };

            var results = jsonElement.EnumerateArray();

            foreach (var result in results)
            {
                await ProcessPersonAsync(result, characters, personEpisodesList);
            }

            apiUrl = json.RootElement.GetProperty("info")
                .GetProperty("next")
                .GetString();

            apiUrl = !string.IsNullOrEmpty(apiUrl) ? apiUrl + $"&name={HttpUtility.UrlPathEncode(name)}" : apiUrl;
        }

        return new PersonParseResult()
        {
            Persons = characters,
            PersonEpisodes = personEpisodesList
        };
    }

    private async Task ProcessPersonAsync(JsonElement result, List<Person> characters, List<PersonEpisodes> personEpisodesList)
    {
        PersonOrigin personOrigin = await GetPersonOriginAsync(result).ConfigureAwait(false);

        var character = new Person
        {
            Id = result.GetProperty("id").GetInt32(),
            Name = result.GetProperty("name").GetString(),
            Status = result.GetProperty("status").GetString(),
            Species = result.GetProperty("species").GetString(),
            Type = result.GetProperty("type").GetString(),
            Origin = personOrigin
        };

        characters.Add(character);

        var episodes = result.GetProperty("episode")
            .EnumerateArray()
            .Select(x =>
            {
                return int.Parse(x.GetString()
                    .Split('/')
                    .Last());
            });

        var personEpisodes = new PersonEpisodes
        {
            PersonId = result.GetProperty("id").GetInt32(),
            EpisodeIds = episodes
        };

        personEpisodesList.Add(personEpisodes);
    }


    private async Task<PersonOrigin> GetPersonOriginAsync(JsonElement result)
    {
        string originUrl = result.GetProperty("origin").GetProperty("url").GetString();

        var personOrigin = new PersonOrigin
        {
            Name = "",
            Type = "",
            Dimension = ""
        };

        if (!string.IsNullOrEmpty(originUrl))
        {
            var httpClient = _httpClientFactory.CreateClient();

            HttpResponseMessage originResponse = await httpClient.GetAsync(originUrl).ConfigureAwait(false);
            string originContent = await originResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            JsonDocument originJson = JsonDocument.Parse(originContent);

            personOrigin.Name = originJson.RootElement.GetProperty("name").GetString();
            personOrigin.Type = originJson.RootElement.GetProperty("type").GetString();
            personOrigin.Dimension = originJson.RootElement.GetProperty("dimension").GetString();
        }

        return personOrigin;
    }
}
