using System.Threading.Tasks;
using RickMortyApp.Entities;

namespace RickMortyApp.Contracts;

public interface IPersonService
{
    Task<CheckPersonInEpisodeResult> CheckPersonInEpisode(string personName, string episodeName);

    Task<GetPersonResult> GetPersonByName(string personName);
}
