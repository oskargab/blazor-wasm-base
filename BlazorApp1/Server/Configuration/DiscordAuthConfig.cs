using System.ComponentModel.DataAnnotations;

namespace winus.Server.Configuration
{
    public class DiscordAuthConfig
    {
        public const string SectionName = "DiscordAuth";
        [MinLength(1)]
        public required string ClientId { get; init; }
        [MinLength(1)]
        public required string ClientSecret { get; init; }
    }
}
