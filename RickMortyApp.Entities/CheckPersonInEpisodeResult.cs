namespace RickMortyApp.Entities;

public class CheckPersonInEpisodeResult
{
    public bool? IsInEpisode { get; set; }

    public bool IsNotFound { get; set; } = false;
}
